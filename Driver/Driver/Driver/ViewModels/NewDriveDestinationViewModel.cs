using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveDestinationViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private DialogService _dialogService;

        public ICommand OnNextButtonClicked => new Command(async () => await NextPage());
        public Drive Drive { get; set; }
        public string DriveDest { get; set; }

        public NewDriveDestinationViewModel(Drive drive, INavigation navigation)
        {
            Drive = drive;
            _navigation = navigation;
            _dialogService = new DialogService();
        }

        async Task NextPage()
        {
            Drive.Destination = DriveDest;

            GetPersonFriendsResponse getPersonFriendsResponse;
            try
            {
                getPersonFriendsResponse = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = Drive.Driver.Username
                }));
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            var friends = getPersonFriendsResponse.Friends.Select(o => (Friend)o);
            var observableFriends = new ObservableCollection<Friend>(friends);

            await _navigation.PushAsync(new NewDriveParticipantsPage(Drive, observableFriends));
        }
    }
}