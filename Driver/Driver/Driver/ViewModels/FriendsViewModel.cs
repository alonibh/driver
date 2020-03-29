using Driver.API;
using Driver.Helpers;
using Driver.Models;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class FriendsViewModel : BaseViewModel
    {
        private readonly IDbHelper _dbHelper;
        private readonly IDialogService _dialogService;
        private readonly string _username;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ObservableCollection<Friend> ApprovedFriends { get; set; }
        public ObservableCollection<Friend> PendingFriends { get; set; }

        public FriendsViewModel(ObservableCollection<Friend> friends, string username)
        {
            _username = username;
            PendingFriends = new ObservableCollection<Friend>();
            ApprovedFriends = new ObservableCollection<Friend>();
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
            AddFriends(friends);
        }

        public async void OnApprovedFriendTapped(object sender, ItemTappedEventArgs args)
        {
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            Friend friend = (args.Item as Friend);
            bool response = await _dialogService.ShowMessage($"Are you sure you want to remove {friend.FullName} as your friend?",
                                                             "Remove Friend", "Yes", "No", null);
            if (response)
            {
                await _dbHelper.DeleteFriend(new DeleteFriendRequest
                {
                    Username = friend.Username
                });

                ReloadFriends();
            }
        }

        public async void OnPendingFriendTapped(object sender, ItemTappedEventArgs args)
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

        void AddFriends(IEnumerable<Friend> friends)
        {
            PendingFriends.Clear();
            ApprovedFriends.Clear();
            foreach (var friend in friends)
            {
                if (friend.Status == FriendRequestStatus.Accepted)
                {
                    ApprovedFriends.Add(friend);
                }
                else
                {
                    PendingFriends.Add(friend);
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