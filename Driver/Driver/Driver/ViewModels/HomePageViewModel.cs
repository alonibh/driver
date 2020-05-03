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
        public string Balance { get; set; }
        public int GlobalDrivesCounter => Person.Drives.Count;
        public int GlobalUserDroveCounter => Person.Drives.Where(o => o.Driver.Username == MainPage.Person.Username).Count();
        public int GlobalUserGotDrivenCounter => Person.Drives.Where(o => o.Driver.Username != MainPage.Person.Username).Count();
        public Person Person { get; set; }

        public HomePageViewModel(INavigation navigation)
        {
            _navigation = navigation;

            Person = MainPage.Person;
            DriveCounters = new ObservableCollection<DriveCounter>(Person.DrivesCounter);

            UpdateCounters();

        }

        private void UpdateCounters()
        {
            int balance = 0;
            foreach (var drive in Person.Drives)
            {
                if (drive.Driver.Username == MainPage.Person.Username)
                {
                    int participants = drive.Participants.Where(o => o.Username != MainPage.Person.Username).Count();
                    balance += participants;
                }
                else
                {
                    balance--;
                }
            }
            if (balance == 0)
            {
                Balance = "You're even";
            }
            else
            {
                Balance = balance > 0 ? $"You get {balance} drives back" : $"You owe {balance * -1} drives";
            }
        }

        private async Task ShowNewDrivePage()
        {
            await _navigation.PushAsync(new NewDriveParticipantsPage());
        }
    }
}