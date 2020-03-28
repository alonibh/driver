using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveDestinationViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly RemoteDbHelper _dbHelper;

        public ICommand OnNextButtonClicked => new Command(async () => await NextPage());
        public Drive Drive { get; set; }
        public string DriveDest { get; set; }

        public NewDriveDestinationViewModel(Drive drive, INavigation navigation)
        {
            Drive = drive;
            _navigation = navigation;
            _dbHelper = new RemoteDbHelper();
        }

        async Task NextPage()
        {
            Drive.Destination = DriveDest;

            GetPersonFriendsResponse getPersonFriendsResponse = await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = Drive.Driver.Username
            });

            var friends = getPersonFriendsResponse.Friends.Select(o => (Friend)o);
            var observableFriends = new ObservableCollection<Friend>(friends);

            await _navigation.PushAsync(new NewDriveParticipantsPage(Drive, observableFriends));
        }
    }
}