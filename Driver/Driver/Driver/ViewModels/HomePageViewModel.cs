using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDbHelper _dbHelper;

        private ObservableCollection<DriveCounter> _driveCounters;
        public ObservableCollection<DriveCounter> DriveCounters
        {
            get { return _driveCounters; }
            set
            {
                _driveCounters = new ObservableCollection<DriveCounter>(value.OrderByDescending(o => o.Counter));
                OnPropertyChanged();
            }
        }

        public ICommand OnAddNewDriveButtonClicked => new Command(async () => await ShowNewDrivePage());
        public string Balance =>
                    (GlobalUserDroveCounter - GlobalUserGotDrivenCounter) > 0 ? $"You get {(GlobalUserDroveCounter - GlobalUserGotDrivenCounter)} drives back" : $"You owe {(GlobalUserDroveCounter - GlobalUserGotDrivenCounter) * -1} drives";
        public int GlobalDrivesCounter => Person.Drives.Count;
        public int GlobalUserDroveCounter { get; set; }
        public int GlobalUserGotDrivenCounter { get; set; }
        public Person Person { get; set; }

        public HomePageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dbHelper = DependencyService.Get<IDbHelper>();

            Person = MainPage.Person;
            DriveCounters = new ObservableCollection<DriveCounter>(Person.DrivesCounter);

            UpdateCounters();

        }

        private void UpdateCounters()
        {
            GlobalUserDroveCounter = 0;
            GlobalUserGotDrivenCounter = 0;

            foreach (var drive in Person.Drives)
            {
                if (drive.Driver.Username == MainPage.Person.Username)
                {
                    int participants = drive.Participants.Where(o => o.Username != MainPage.Person.Username).Count();
                    GlobalUserDroveCounter += participants;
                }
                else
                {
                    GlobalUserGotDrivenCounter++;
                }
            }
        }

        private async Task ShowNewDrivePage()
        {
            await _navigation.PushAsync(new NewDriveParticipantsPage());
        }
    }
}