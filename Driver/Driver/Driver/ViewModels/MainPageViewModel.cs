using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using Plugin.Toast;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDbHelper _dbHelper;

        public ICommand OnLogoutButtonClicked => new Command(async () => await Logout());
        public ICommand OnFriendsListButtonClicked => new Command(async () => await ShowFriends());
        public ICommand OnNewDriveButtonClicked => new Command(async () => await AddDrive());
        public Person Person { get; set; }

        public MainPageViewModel(Person person, INavigation navigation)
        {
            Person = person;
            _navigation = navigation;
            _dbHelper = DependencyService.Get<IDbHelper>();
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

        async Task ShowFriends()
        {

            GetPersonFriendsResponse getPersonFriendsResponse = await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = Person.Username
            });

            await _navigation.PushAsync(new FriendsTabbedPage(getPersonFriendsResponse.Friends.Select(o => (Friend)o), Person.Username));
        }

        async Task AddDrive()
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

            await _navigation.PushAsync(new NewDriveDestinationPage(drive));
        }

        async Task Logout()
        {
            _dbHelper.SetToken(null);
            Application.Current.Properties.Remove("username");
            Application.Current.Properties.Remove("token");
            CrossToastPopUp.Current.ShowToastMessage("Successfully logged out");
            var currPage = _navigation.NavigationStack.Single();
            await _navigation.PushAsync(new LoginPage());
            _navigation.RemovePage(currPage);
        }
    }
}