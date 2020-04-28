using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveParticipantsViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IEnumerable<FriendWithCheckBox> _defaultFriends;
        private readonly Drive _drive;

        private ObservableCollection<FriendWithCheckBox> _friends;
        public ObservableCollection<FriendWithCheckBox> Friends
        {
            get
            {
                return _friends;
            }
            set
            {
                _friends = new ObservableCollection<FriendWithCheckBox>(value.OrderBy(o => o.Friend.FullName));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Friend> SelectedFriends { get; set; }

        public ICommand OnNextButtonClicked => new Command(async () => await MoveToNextPage());
        public ICommand PerformSearch => new Command<string>((string query) => SearchFriend(query));

        public NewDriveParticipantsViewModel(Drive drive, IEnumerable<Friend> friends, INavigation navigation)
        {
            _navigation = navigation;
            _drive = drive;

            var friendsWithCheckBox = new List<FriendWithCheckBox>();
            foreach (var friend in friends)
            {
                friendsWithCheckBox.Add(new FriendWithCheckBox { Friend = friend, IsChecked = false });
            }

            _defaultFriends = new ObservableCollection<FriendWithCheckBox>(friendsWithCheckBox);
            Friends = new ObservableCollection<FriendWithCheckBox>(friendsWithCheckBox);

            foreach (var friend in Friends)
            {
                friend.PropertyChanged += OnCheckBoxCheckedChanged;
            }

            SelectedFriends = new ObservableCollection<Friend>();
        }

        public void OnFriendTapped(object sender, SelectionChangedEventArgs args)
        {
            if (!args.CurrentSelection.Any())
            {
                return;
            }

            FriendWithCheckBox friendWithCheckBox = args.CurrentSelection[0] as FriendWithCheckBox;

            CollectionView cv = (CollectionView)sender;
            cv.SelectedItem = null;

            if (friendWithCheckBox.IsChecked)
            {
                SelectedFriends.Remove(friendWithCheckBox.Friend);
                friendWithCheckBox.IsChecked = false;
            }
            else
            {
                SelectedFriends.Add(friendWithCheckBox.Friend);
                friendWithCheckBox.IsChecked = true;
            }
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e) => SearchFriend(e.NewTextValue);

        private void OnCheckBoxCheckedChanged(object sender, PropertyChangedEventArgs e)
        {
            FriendWithCheckBox friendWithCheckBox = sender as FriendWithCheckBox;
            if (friendWithCheckBox.IsChecked)
            {
                if (!SelectedFriends.Contains(friendWithCheckBox.Friend))
                {
                    SelectedFriends.Add(friendWithCheckBox.Friend);
                }
            }
            else
            {
                if (SelectedFriends.Contains(friendWithCheckBox.Friend))
                {
                    SelectedFriends.Remove(friendWithCheckBox.Friend);
                }
            }
        }

        private void SearchFriend(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                Friends = new ObservableCollection<FriendWithCheckBox>(_defaultFriends);
            }
            else
            {
                var queryFriends = Friends.Where(o => o.Friend.FullName.StartsWith(query, StringComparison.OrdinalIgnoreCase));
                Friends = new ObservableCollection<FriendWithCheckBox>(queryFriends);
            }
        }

        private async Task MoveToNextPage()
        {
            _drive.Participants = new List<DriveParticipant>();
            foreach (var friend in SelectedFriends)
            {
                _drive.Participants.Add(new DriveParticipant
                {
                    Username = friend.Username,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    Address = friend.Address
                });
            }

            _drive.Participants.Add(new DriveParticipant
            {
                Username = _drive.Driver.Username,
                FirstName = _drive.Driver.FirstName,
                LastName = _drive.Driver.LastName,
                Address = _drive.Driver.Address
            });

            await _navigation.PushAsync(new NewDriveSummaryPage(_drive));
        }
    }
}