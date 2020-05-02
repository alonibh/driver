using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class ApprovedFriendsViewModel : BaseViewModel
    {
        private readonly IDbHelper _dbHelper;
        private readonly string _username;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ObservableCollection<FriendDrivesCounter> ApprovedFriends { get; set; }

        public ApprovedFriendsViewModel()
        {
            _username = MainPage.Person.Username;
            _dbHelper = DependencyService.Get<IDbHelper>();
            ApprovedFriends = new ObservableCollection<FriendDrivesCounter>();
            AddFriends(MainPage.Person.Friends, MainPage.Person.Drives);
        }

        public void SubscribePoppingEvent()
        {
            PopupNavigation.Instance.Popping += Instance_Popping;
        }

        public void UnsubscribePoppingEvent()
        {
            PopupNavigation.Instance.Popping -= Instance_Popping;
        }

        public async void OnApprovedFriendTapped(object sender, SelectionChangedEventArgs args)
        {
            if (!args.CurrentSelection.Any())
            {
                return;
            }

            Friend friend = (args.CurrentSelection[0] as FriendDrivesCounter).Friend;

            CollectionView cv = (CollectionView)sender;
            cv.SelectedItem = null;

            var drives = new List<Drive>(MainPage.Person.Drives.Where(d => d.Participants.Exists(p => p.Username == friend.Username)));

            await PopupNavigation.Instance.PushAsync(new FriendPopupPage(friend, drives)
            {
                HasSystemPadding = true,
                Padding = 10
            });
        }

        public async Task ReloadFriends()
        {
            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = _username
            })).Friends.Select(o => (Friend)o);

            var drives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = _username
            })).Drives.Select(o => (Drive)o);

            AddFriends(friends, drives);
        }

        async void Instance_Popping(object sender, Rg.Plugins.Popup.Events.PopupNavigationEventArgs e)
        {
            await ReloadFriends();
        }

        async Task RefreshItemsAsync()
        {
            IsBusy = true;

            await ReloadFriends();

            IsBusy = false;
        }

        void AddFriends(IEnumerable<Friend> friends, IEnumerable<Drive> drives)
        {
            ApprovedFriends.Clear();
            Dictionary<string, int> drivesCounter = new Dictionary<string, int>();
            foreach (var drive in drives)
            {
                if (drive.Driver.Username == _username)
                {
                    foreach (var participant in drive.Participants)
                    {
                        if (!drivesCounter.ContainsKey(participant.Username))
                        {
                            drivesCounter.Add(participant.Username, 0);
                        }

                        drivesCounter[participant.Username]++;
                    }
                }
                else
                {
                    if (!drivesCounter.ContainsKey(drive.Driver.Username))
                    {
                        drivesCounter.Add(drive.Driver.Username, 0);
                    }

                    drivesCounter[drive.Driver.Username]--;
                }
            }
            foreach (var friend in friends)
            {
                if (friend.Status == FriendRequestStatus.Accepted)
                {
                    int counter;
                    drivesCounter.TryGetValue(friend.Username, out counter);
                    ApprovedFriends.Add(new FriendDrivesCounter
                    {
                        Friend = friend,
                        Counter = counter
                    });
                }
            }
        }
    }
}