using Driver.API;
using Driver.Helpers;
using Driver.Models;
using GalaSoft.MvvmLight.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class MainPage : MasterDetailPage
    {
        readonly MasterPage _masterPage;
        readonly Person _person;
        readonly IDbHelper _dbHelper;
        readonly IDialogService _dialogService;

        public MainPage(Person person)
        {
            InitializeComponent();

            _person = person;
            _masterPage = new MasterPage();
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();

            Master = _masterPage;
            Detail = new NavigationPage(new HomePage(_person))
            {
                BarBackgroundColor = Color.FloralWhite,
                BarTextColor = Color.Black
            };

            _masterPage.listView.ItemSelected += OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (item.TargetType != Detail.Navigation.NavigationStack[0].GetType())
                {
                    switch (item.TargetType.Name)
                    {
                        case nameof(HomePage):
                            {
                                Detail = new NavigationPage(new HomePage(_person))
                                {
                                    BarBackgroundColor = Color.FloralWhite,
                                    BarTextColor = Color.Black
                                };
                                break;
                            }
                        case nameof(LoginPage):
                            {
                                await Logout();
                                break;
                            }
                        case nameof(FriendsTabbedPage):
                            {
                                await ShowFriends();
                                break;
                            }
                        case nameof(NewDriveDestinationPage):
                            {
                                await AddDrive();
                                break;
                            }
                    }
                }
                _masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }

        async Task Logout()
        {
            bool answer = await _dialogService.ShowMessage("Are you sure you want to logout?", "Logout", "Yes", "No", null);
            if (answer)
            {
                _dbHelper.SetToken(null);
                Application.Current.Properties.Remove("username");
                Application.Current.Properties.Remove("token");

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        async Task ShowFriends()
        {
            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = _person.Username
            })).Friends.Select(o => (Friend)o);

            var drives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = _person.Username
            })).Drives.Select(o => (Drive)o);

            await ((NavigationPage)Detail).PushAsync(new FriendsTabbedPage(friends, drives, _person.Username));
        }

        async Task AddDrive()
        {
            var drive = new Drive
            {
                Date = DateTime.Now,
                Driver = new DriveParticipant
                {
                    Username = _person.Username,
                    FirstName = _person.FirstName,
                    LastName = _person.LastName,
                    Address = _person.Address
                }
            };

            await ((NavigationPage)Detail).PushAsync(new NewDriveDestinationPage(drive));
        }
    }
}