using Driver.API;
using Driver.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveDestinationPage : ContentPage
    {
        public NewDriveDestinationPage()
        {
            InitializeComponent();
        }

        async void OnNextButtonClicked(object sender, EventArgs args)
        {
            var drive = (Drive)BindingContext;
            drive.Destination = driveDestEntry.Text;

            GetPersonFriendsResponse getPersonFriendsResponse;
            try
            {
                getPersonFriendsResponse = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = drive.Driver.Username
                }));
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            var friends = getPersonFriendsResponse.Friends.Select(o => (Friend)o);
            var observableFriends = new ObservableCollection<Friend>(friends);

            await Navigation.PushAsync(new NewDriveParticipantsPage(observableFriends)
            {
                BindingContext = drive
            });
        }
    }
}