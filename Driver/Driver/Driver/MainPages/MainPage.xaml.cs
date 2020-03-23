using Driver.API;
using Driver.LoginPages;
using Driver.Models;
using Driver.NewDrivePages;
using Plugin.Toast;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace Driver.MainPages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            DataGridComponent.Init();
        }

        async void OnDriveTapped(object sender, ItemTappedEventArgs itemTapped)
        {
            var person = (Person)BindingContext;

            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            string driveId = (itemTapped.Item as Drive).Id;
            GetDriveResponse getDriveResponse;
            try
            {
                getDriveResponse = await App.Database.GetDrive(new GetDriveRequest
                {
                    DriveId = driveId
                });
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            DriveInfo driveInfo = new DriveInfo
            {
                Drive = getDriveResponse.Drive,
                Username = person.Username
            };

            await Navigation.PushAsync(new DriveInfoPage()
            {
                BindingContext = driveInfo
            });
        }

        async void OnLogoutButtonClicked(object sender, EventArgs args)
        {
            App.Database.SetToken(null);
            Application.Current.Properties.Remove("username");
            Application.Current.Properties.Remove("token");
            CrossToastPopUp.Current.ShowToastMessage("Successfully logged out");
            var currPage = Navigation.NavigationStack.Single();
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(currPage);
        }

        async void OnFriendsListButtonClicked(object sender, EventArgs args)
        {
            var person = (Person)BindingContext;

            GetPersonFriendsResponse getPersonFriendsResponse;
            try
            {
                getPersonFriendsResponse = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = person.Username
                }));
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            await Navigation.PushAsync(new FriendsTabbedPage(getPersonFriendsResponse.Friends.Select(o => (Friend)o), person.Username));
        }

        async void OnNewDriveButtonClicked(object sender, EventArgs args)
        {
            var person = (Person)BindingContext;
            var drive = new Drive
            {
                Date = DateTime.Now,
                Driver = new DriveParticipant
                {
                    Username = person.Username,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Address = person.Address
                }
            };

            await Navigation.PushAsync(new NewDriveDestinationPage()
            {
                BindingContext = drive
            });
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result)
                    DependencyService.Get<IAndroidMethods>().CloseApp();
            });

            return true;
        }
    }

    public interface IAndroidMethods
    {
        void CloseApp();
    }
}