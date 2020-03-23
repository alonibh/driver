using Driver.API;
using Driver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModel
{
    public class FriendsPageViewModel : INotifyPropertyChanged
    {
        bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Friend> PendingFriends { get; private set; }
        public ObservableCollection<Friend> ApprovedFriends { get; private set; }
        public string Username { get; private set; }

        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());

        public FriendsPageViewModel(ObservableCollection<Friend> friends, string username)
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
                    ApprovedFriends.Add(friend);
                else
                    PendingFriends.Add(friend);
            }
        }

        async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
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

            IsRefreshing = false;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}