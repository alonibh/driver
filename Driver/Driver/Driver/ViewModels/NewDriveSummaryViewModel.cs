using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveSummaryViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;
        private readonly Drive _drive;

        public ObservableCollection<DriveParticipant> Participants { get; set; }

        public ICommand OnDoneButtonClicked => new Command(async () => await AddDrive());

        public NewDriveSummaryViewModel(Drive drive, INavigation navigation)
        {
            _drive = drive;
            _navigation = navigation;
            _dialogService = DependencyService.Get<IDialogService>();
            _dbHelper = DependencyService.Get<IDbHelper>();

            Participants = new ObservableCollection<DriveParticipant>();

            foreach (var participant in _drive.Participants)
            {
                if (participant.Username == _drive.Driver.Username)
                {
                    break;
                }

                Participants.Add(new DriveParticipant
                {
                    Username = participant.Username,
                    FirstName = participant.FirstName,
                    LastName = participant.LastName,
                    Address = participant.Address
                });
            }
        }

        public void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _drive.Date = e.NewDate;
        }

        public void OnDestChanged(object sender, TextChangedEventArgs e)
        {
            _drive.Destination = e.NewTextValue;
        }

        async Task AddDrive()
        {
            AddDriveResponse addDriveResponse = await _dbHelper.AddDrive(new AddDriveRequest
            {
                Driver = _drive.Driver.Username,
                Participants = _drive.Participants.Select(o => o.Username).ToList(),
                Date = _drive.Date,
                Dest = _drive.Destination
            });

            if (!addDriveResponse.Success)
            {
                await _dialogService.ShowMessage("Unable to add drive", "Error", "OK", null);
                return;
            }

            var drives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = _drive.Driver.Username
            })).Drives.Select(o => (Drive)o);

            MainPage.Person.Drives = drives.ToList();

            await _navigation.PopToRootAsync();
        }
    }
}