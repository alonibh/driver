using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class DriveInfoViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        public ICommand OnDeleteDriveButtonClicked => new Command(async () => await DeleteDrive());
        public Drive Drive { get; set; }
        public string Username { get; }
        public bool IsUserDriver => Drive.Driver.Username == Username;

        public DriveInfoViewModel(Drive drive, string username, INavigation navigation)
        {
            Drive = drive;
            Username = username;
            _navigation = navigation;
            _dialogService = DependencyService.Get<IDialogService>();
            _dbHelper = DependencyService.Get<IDbHelper>();
        }

        async Task DeleteDrive()
        {
            bool answer = await _dialogService.ShowMessage("Delete Drive", "Are you sure you want to delete this drive?", "Yes", "No", null);
            if (answer)
            {
                DeleteDriveResponse deleteDriveResponse = await _dbHelper.DeleteDrive(new DeleteDriveRequest
                {
                    DriveId = Drive.Id
                });

                if (!deleteDriveResponse.Success)
                {
                    await _dialogService.ShowMessage("Error", "Failed to delete drive", "OK", null);
                    return;
                }

                else
                {
                    GetPersonResponse getPersonResponse = await _dbHelper.GetPerson(new GetPersonRequest
                    {
                        Username = Username
                    });

                    GetPersonDrivesResponse getPersonDrivesResponse = await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
                    {
                        Username = Username
                    });

                    Person person = new Person
                    {
                        Username = getPersonResponse.Person.Username,
                        Address = getPersonResponse.Person.Address,
                        FirstName = getPersonResponse.Person.FirstName,
                        LastName = getPersonResponse.Person.LastName,
                        Email = getPersonResponse.Person.Email,
                        Drives = getPersonDrivesResponse.Drives.Select(o => (Drive)o).ToList(),
                        Friends = new List<Friend>()
                    };

                    MainPage mainPage = new MainPage(person);

                    var rootPage = _navigation.NavigationStack[0];
                    _navigation.InsertPageBefore(mainPage, rootPage);
                    await _navigation.PopToRootAsync();
                }
            }
        }
    }
}