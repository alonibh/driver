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
    public class FriendsViewModel : BaseViewModel
    {
        private readonly string _username;
        private readonly IDbHelper _dbHelper;

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ObservableCollection<Friend> PendingFriends { get; private set; }
        public ObservableCollection<Friend> ApprovedFriends { get; private set; }

        public FriendsViewModel(ObservableCollection<Friend> friends, string username)
        {
            _username = username;
            PendingFriends = new ObservableCollection<Friend>();
            ApprovedFriends = new ObservableCollection<Friend>();
            _dbHelper = DependencyService.Get<IDbHelper>();
            AddFriends(friends);
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