using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class SearchPersonViewModel : BaseViewModel
    {
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;
        private string _lastQuery;
        private readonly Dictionary<string, int> _groupOrder;

        public ObservableCollection<FriendGroup> Friends { get; set; }
        public ICommand PerformSearch => new Command<string>(async (string query) => await SearchPerson(query));
        public ICommand OnUnfriendButtonClicked => new Command<Friend>(async (Friend friend) => await Unfriend(friend));
        public ICommand OnAddFriendButtonClicked => new Command<string>(async (string username) => await AddFriend(username));
        public ICommand OnIgnoreFriendRequestButtonClicked => new Command<string>(async (string username) => await DeleteFriend(username));

        public SearchPersonViewModel()
        {
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
            _groupOrder = new Dictionary<string, int>
            {
                { "Existing Friends", 0 },
                { "Pending Requests", 1},
                { "Waiting For Approval", 2},
                { "All", 3}
            };

            // { ("Existing Friends",1), "Pending Requests", "Waiting For Approval", "All" };
            Friends = new ObservableCollection<FriendGroup>();
        }

        public async void OnTextChanged(object sender, TextChangedEventArgs e) =>
            await SearchPerson(e.NewTextValue);

        private async Task DeleteFriend(string username)
        {
            await _dbHelper.DeleteFriend(new DeleteFriendRequest
            {
                Username = username
            });

            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = MainPage.Person.Username
            })).Friends.Select(o => (Friend)o);

            MainPage.Person.Friends = friends.ToList();

            await SearchPerson(_lastQuery);
        }

        private async Task AddFriend(string username)
        {
            await _dbHelper.AddFriend(new AddFriendRequest
            {
                Username = username
            });

            var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = MainPage.Person.Username
            })).Friends.Select(o => (Friend)o);

            MainPage.Person.Friends = friends.ToList();

            await SearchPerson(_lastQuery);
        }

        private async Task Unfriend(Friend friend)
        {
            bool answer = await _dialogService.ShowMessage($"Are you sure you want to cancel your friendship with {friend.FullName}?", "Cancel friendship", "Yes", "No", null);
            if (answer)
            {
                await _dbHelper.DeleteFriend(new DeleteFriendRequest
                {
                    Username = friend.Username
                });

                var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = MainPage.Person.Username
                })).Friends.Select(o => (Friend)o);

                MainPage.Person.Friends = friends.ToList();

                await SearchPerson(_lastQuery);
            }
        }

        private async Task SearchPerson(string query)
        {
            await semaphoreSlim.WaitAsync();
            _lastQuery = query;
            try
            {
                if (!string.IsNullOrEmpty(query) && query.Length > 2)
                {
                    var SearchPersonResponse = await _dbHelper.SearchPerson(new SearchPersonRequest
                    {
                        Query = query
                    });

                    var queryFriends = SearchPersonResponse.Results.Select(o => (Friend)o);
                    UpdateFriendsResults(queryFriends);
                }
                else
                {
                    Friends.Clear();
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private void UpdateFriendsResults(IEnumerable<Friend> friends)
        {
            List<Friend> existingFriends = new List<Friend>();
            List<Friend> pendingFriends = new List<Friend>();
            List<Friend> waitingForApprovalFriends = new List<Friend>();
            List<Friend> nonFriends = new List<Friend>();

            foreach (var friend in friends)
            {
                switch (friend.Status)
                {
                    case FriendRequestStatus.Accepted:
                        existingFriends.Add(friend);
                        break;
                    case FriendRequestStatus.Pending:
                        pendingFriends.Add(friend);
                        break;
                    case FriendRequestStatus.WaitingForApproval:
                        waitingForApprovalFriends.Add(friend);
                        break;
                    case FriendRequestStatus.NotFriedns:
                        nonFriends.Add(friend);
                        break;
                }
            }

            UpdateGroup(existingFriends, "Existing Friends");
            UpdateGroup(pendingFriends, "Pending Requests");
            UpdateGroup(waitingForApprovalFriends, "Waiting For Approval");
            UpdateGroup(nonFriends, "All");
        }

        private void UpdateGroup(List<Friend> friendsList, string groupName)
        {
            var group = Friends.SingleOrDefault(o => o.Name == groupName);
            if (friendsList.Any())
            {
                if (group == null)
                {
                    SafeInsert(new FriendGroup(groupName, friendsList));
                }
                else
                {
                    List<Friend> toRemove = new List<Friend>();
                    List<Friend> toAdd = new List<Friend>(friendsList);

                    foreach (var friend in group)
                    {
                        if (!friendsList.Exists(o => o.Username == friend.Username))
                        {
                            toRemove.Add(friend);
                        }
                        else
                        {
                            toAdd.RemoveAll(o => o.Username == friend.Username);
                        }
                    }

                    foreach (var friend in toRemove)
                    {
                        group.Remove(friend);
                    }

                    foreach (var friend in toAdd)
                    {
                        group.Add(friend);
                    }
                }
            }
            else
            {
                if (group != null)
                {
                    Friends.Remove(group);
                }
            }
        }

        private void SafeInsert(FriendGroup friendGroup)
        {
            int counter = 0;
            foreach (var group in Friends)
            {
                int currentIndex = _groupOrder[group.Name];
                int desiredIndex = _groupOrder[friendGroup.Name];
                if (desiredIndex < currentIndex)
                {
                    Friends.Insert(counter, friendGroup);
                    return;
                }
                counter++;
            }
            Friends.Add(friendGroup);
        }
    }

    public class FriendGroup : ObservableCollection<Friend>
    {
        public string Name { get; private set; }

        public FriendGroup(string name, List<Friend> friends) : base(friends)
        {
            Name = name;
        }
    }
}