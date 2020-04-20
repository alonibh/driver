using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private DriveCounter _currentDriveCounter;
        public DriveCounter CurrentDriveCounter
        {
            get { return _currentDriveCounter; }
            set
            {
                _currentDriveCounter = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnAddNewDriveButtonClicked => new Command(() => ShowNewDrivePage());

        public int GlobalDrivesCounter => GetGlobalDrivesCounter();

        public Person Person { get; set; }

        public HomePageViewModel(Person person, INavigation navigation)
        {
            _navigation = navigation;
            _dbHelper = DependencyService.Get<IDbHelper>();
            Person = person;

            DriveCounters = new ObservableCollection<DriveCounter>(Person.DrivesCounter);
            if (DriveCounters.Count > 0)
            {
                CurrentDriveCounter = DriveCounters[0];
            }
        }

        public async void OnDriveTapped(object sender, ItemTappedEventArgs args)
        {
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            string driveId = (args.Item as Drive).Id;
            GetDriveResponse getDriveResponse = await _dbHelper.GetDrive(new GetDriveRequest
            {
                DriveId = driveId
            });

            Drive drive = new Drive
            {
                Id = getDriveResponse.Drive._id,
                Date = getDriveResponse.Drive.Date,
                Destination = getDriveResponse.Drive.Dest,
                Driver = getDriveResponse.Drive.Driver,
                Participants = getDriveResponse.Drive.Participants.Select(o => (DriveParticipant)o).ToList()
            };

            await _navigation.PushAsync(new DriveInfoPage(drive, Person.Username));
        }

        private int GetGlobalDrivesCounter()
        {
            int counter = 0;
            foreach (var driveCounter in DriveCounters)
            {
                counter += driveCounter.Counter;
            }
            return counter;
        }

        private void ShowNewDrivePage()
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

            ((MasterDetailPage)App.Current.MainPage).Detail = new NavigationPage(new NewDriveDestinationPage(drive))
            {
                BarBackgroundColor = Color.LightGray,
                BarTextColor = Color.Black
            };
        }
    }
}