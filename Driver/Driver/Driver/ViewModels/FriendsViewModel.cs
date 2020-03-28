using Driver.API;
using Driver.Models;
using MvvmHelpers;
using System;
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
        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());
        public ObservableCollection<Friend> PendingFriends { get; private set; }
        public ObservableCollection<Friend> ApprovedFriends { get; private set; }
        public string Username { get; private set; }

        public FriendsViewModel(ObservableCollection<Friend> friends, string username)
        {
            Username = username;
            PendingFriends = new ObservableCollection<Friend>();
            ApprovedFriends = new ObservableCollection<Friend>();
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

            try
            {
                var getPersonFriendsResponse = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = Username
                }));
                AddFriends(getPersonFriendsResponse.Friends.Select(o => (Friend)o));
            }
            catch (Exception)
            { }

            IsBusy = false;
        }
    }
}