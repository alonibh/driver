using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class DrivesHistoryViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        public ICommand OnDeleteDriveButtonClicked => new Command(async (driveId) => await DeleteDrive(driveId as string));
        public ObservableCollection<ObservableDrive> Drives { get; set; }

        public DrivesHistoryViewModel()
        {
            _dialogService = DependencyService.Get<IDialogService>();
            _dbHelper = DependencyService.Get<IDbHelper>();

            var observableDrives = new List<ObservableDrive>();
            foreach (var drive in MainPage.Person.Drives)
            {
                observableDrives.Add(new ObservableDrive
                {
                    Id = drive.Id,
                    Driver = drive.Driver,
                    Date = drive.Date,
                    Destination = drive.Destination,
                    Participants = new ObservableCollection<DriveParticipant>(drive.Participants.Where(o => o.Username != drive.Driver.Username))
                });
            }
            Drives = new ObservableCollection<ObservableDrive>(observableDrives.OrderByDescending(o => o.Date));
        }

        async Task DeleteDrive(string driveId)
        {
            bool answer = await _dialogService.ShowMessage("Are you sure you want to delete this drive?", "Delete Drive", "Yes", "No", null);
            if (answer)
            {
                DeleteDriveResponse deleteDriveResponse = await _dbHelper.DeleteDrive(new DeleteDriveRequest
                {
                    DriveId = driveId
                });

                if (!deleteDriveResponse.Success)
                {
                    await _dialogService.ShowMessage("Failed to delete drive", "Error", "OK", null);
                    return;
                }

                var drive = Drives.First(o => o.Id == driveId);
                Drives.Remove(drive);

                var drives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = MainPage.Person.Username
                })).Drives.Select(o => (Drive)o);

                MainPage.Person.Drives = drives.ToList();
            }
        }

    }
    public class ObservableDrive
    {
        public string Id { get; set; }
        public bool IsUserDriver => Driver.Username == MainPage.Person.Username;
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<DriveParticipant> Participants { get; set; }
        public DriveParticipant Driver { get; set; }
    }
}