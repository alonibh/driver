using Driver.API;
using Driver.Helpers;
using Driver.Models;
using MvvmHelpers;
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
        private readonly string _username;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ICommand OnAcceptFriendClicked => new Command((friend) => AcceptFriend(friend as Friend));
        public ICommand OnRemoveFriendClicked => new Command((friend) => RemoveFriend(friend as Friend));


        public ObservableCollection<Friend> WaitingForApprovalFriends { get; set; }

        public WaitingForApprovalFriendsViewModel(IEnumerable<Friend> friends, string username)
        {
            _username = username;
            _dbHelper = DependencyService.Get<IDbHelper>();
            WaitingForApprovalFriends = new ObservableCollection<Friend>();
            AddFriends(friends);
        }

        public async void AcceptFriend(Friend friend)
        {
            await _dbHelper.AddFriend(new AddFriendRequest
            {
                Username = friend.Username
            });

            await ReloadFriends();
        }

        public async void RemoveFriend(Friend friend)
        {
            var a = await _dbHelper.DeleteFriend(new DeleteFriendRequest
            {
                Username = friend.Username
            });

            await ReloadFriends();
        }

        public async Task ReloadFriends()
        {
            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = _username
            })).Friends.Select(o => (Friend)o);

            AddFriends(friends);
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
    }
}