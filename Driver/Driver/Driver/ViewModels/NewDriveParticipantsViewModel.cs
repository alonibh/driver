using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveParticipantsViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ObservableCollection<object> _selectedFriends = new ObservableCollection<object>();

        public ICommand OnNextButtonClicked => new Command(async () => await NextPage());
        public Drive Drive { get; set; }
        public ObservableCollection<Friend> ObservableFriends { get; set; }
        public ObservableCollection<object> SelectedFriends
        {
            get
            {
                return _selectedFriends;
            }

            set
            {
                _selectedFriends = value;
                OnPropertyChanged();
            }
        }

        public NewDriveParticipantsViewModel(Drive drive, ObservableCollection<Friend> observableFriends, INavigation navigation)
        {
            Drive = drive;
            ObservableFriends = observableFriends;
            _navigation = navigation;
        }

        async Task NextPage()
        {
            List<DriveParticipant> participants = new List<DriveParticipant>();
            foreach (Friend friend in SelectedFriends)
            {
                participants.Add(new DriveParticipant
                {
                    Username = friend.Username,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    Address = friend.Address,
                });
            }

            Drive.Participants = participants;
            await _navigation.PushAsync(new NewDriveSummaryPage(Drive));
        }
    }
}