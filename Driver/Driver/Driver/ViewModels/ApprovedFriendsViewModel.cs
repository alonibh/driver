using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
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
        private readonly IDialogService _dialogService;
        private readonly string _username;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ObservableCollection<Friend> ApprovedFriends { get; set; }

        public ApprovedFriendsViewModel(ObservableCollection<Friend> friends, string username)
        {
            _username = username;
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
            ApprovedFriends = new ObservableCollection<Friend>();
            AddFriends(friends);
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

            Friend friend = (args.CurrentSelection[0] as Friend);

            CollectionView cv = (CollectionView)sender;
            cv.SelectedItem = null;

            var personDrives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = _username
            })).Drives.Select(o => (Drive)o);

            List<Drive> drives = new List<Drive>();
            drives.AddRange(personDrives.Where(d => d.Participants.Exists(p => p.Username == friend.Username)));

            await PopupNavigation.Instance.PushAsync(new FriendPopupPage(friend, drives));
        }

        public async void OnWaitingForApprovalFriendTapped(object sender, ItemTappedEventArgs args)
        {
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            Friend friend = (args.Item as Friend);
            bool response = await _dialogService.ShowMessage($"Do you want to add {friend.FullName} as your friend?", "Add Friend", "Yes", "No", null);
            if (response)
            {
                await _dbHelper.AddFriend(new AddFriendRequest
                {
                    Username = friend.Username
                });

                ReloadFriends();
            }
        }

        public async void ReloadFriends()
        {
            GetPersonFriendsResponse getPersonFriendsResponse = await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = _username
            });

            AddFriends(getPersonFriendsResponse.Friends.Select(o => (Friend)o));
        }

        void Instance_Popping(object sender, Rg.Plugins.Popup.Events.PopupNavigationEventArgs e)
        {
            ReloadFriends();
        }

        async Task RefreshItemsAsync()
        {
            IsBusy = true;

            GetPersonFriendsResponse getPersonFriendsResponse = await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = _username
            });

            AddFriends(getPersonFriendsResponse.Friends.Select(o => (Friend)o));

            IsBusy = false;
        }

        void AddFriends(IEnumerable<Friend> friends)
        {
            ApprovedFriends.Clear();
            foreach (var friend in friends)
            {
                if (friend.Status == FriendRequestStatus.Accepted)
                {
                    ApprovedFriends.Add(friend);
                }
            }
        }
    }
}