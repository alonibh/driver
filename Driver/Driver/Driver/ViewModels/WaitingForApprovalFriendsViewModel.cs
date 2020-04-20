using Driver.API;
using Driver.Helpers;
using Driver.Models;
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
    public class WaitingForApprovalFriendsViewModel : BaseViewModel
    {
        private readonly IDbHelper _dbHelper;
        private readonly IDialogService _dialogService;
        private readonly string _username;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());

        public ObservableCollection<Friend> WaitingForApprovalFriends { get; set; }

        public WaitingForApprovalFriendsViewModel(ObservableCollection<Friend> friends, string username)
        {
            _username = username;
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
            WaitingForApprovalFriends = new ObservableCollection<Friend>();
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

        void AddFriends(IEnumerable<Friend> friends)
        {
            WaitingForApprovalFriends.Clear();
            foreach (var friend in friends)
            {
                if (friend.Status == FriendRequestStatus.WaitingForApproval)
                {
                    WaitingForApprovalFriends.Add(friend);
                }
            }
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
    }
}