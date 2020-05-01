using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System;
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
        public string GlobalDrivesCounterStr { get; set; }
        public int GlobalDrivesCounter { get; set; }
        public int GlobalUserDroveCounter { get; set; }
        public int GlobalUserGotDrivenCounter { get; set; }
        public Person Person { get; set; }

        public HomePageViewModel(Person person, INavigation navigation)
        {
            _navigation = navigation;
            _dbHelper = DependencyService.Get<IDbHelper>();

            Person = person;
            DriveCounters = new ObservableCollection<DriveCounter>(Person.DrivesCounter);

            SetGlobalDrivesCounter();
        }

        private void SetGlobalDrivesCounter()
        {
            int counter = 0;
            foreach (var driveCounter in DriveCounters)
            {
                if (driveCounter.Counter > 0)
                {
                    GlobalUserDroveCounter += driveCounter.Counter;
                }
                else
                {
                    GlobalUserGotDrivenCounter += driveCounter.Counter;
                }

                counter += driveCounter.Counter;
            }
            GlobalDrivesCounterStr = counter > 0 ? $"You get {counter} drives back" : $"You owe {counter * -1} drives";
            GlobalDrivesCounter = counter;
        }

        private async Task ShowNewDrivePage()
        {
            var drive = new Drive
            {
                Date = DateTime.Now,
                Driver = new DriveParticipant
                {
                    Username = Person.Username,
                    FirstName = Person.FirstName,
                    LastName = Person.LastName,
                    Address = Person.Address
                }
            };

            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = Person.Username
            })).Friends.Select(o => (Friend)o).Where(f => f.Status == FriendRequestStatus.Accepted);

            await _navigation.PushAsync(new NewDriveParticipantsPage(drive, friends));
        }
    }
}