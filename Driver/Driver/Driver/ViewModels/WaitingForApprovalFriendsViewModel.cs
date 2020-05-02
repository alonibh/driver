using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
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
        public ICommand OnAcceptFriendClicked => new Command(async (friend) => await AcceptFriend(friend as Friend));
        public ICommand OnRemoveFriendClicked => new Command(async (friend) => await RemoveFriend(friend as Friend));

        public ObservableCollection<Friend> WaitingForApprovalFriends { get; set; }

        public WaitingForApprovalFriendsViewModel()
        {
            _username = MainPage.Person.Username;
            _dbHelper = DependencyService.Get<IDbHelper>();
            WaitingForApprovalFriends = new ObservableCollection<Friend>();
            AddFriends(MainPage.Person.Friends);
        }

        public async Task AcceptFriend(Friend friend)
        {
            await _dbHelper.AddFriend(new AddFriendRequest
            {
                Username = friend.Username
            });
            await ReloadFriends();
        }

        public async Task RemoveFriend(Friend friend)
        {
            await _dbHelper.DeleteFriend(new DeleteFriendRequest
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

            MainPage.Person.Friends = friends.ToList();

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