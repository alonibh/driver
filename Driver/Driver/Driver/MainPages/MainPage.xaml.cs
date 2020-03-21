using Driver.API;
using Driver.Models;
using Driver.NewDrivePages;
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

        async void OnDriveTapped(object sender, ItemTappedEventArgs e)
        {
            var person = (Person)BindingContext;

            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            int driveId = (e.Item as Drive).Id;
            var drive = (await App.Database.GetDrive(new GetDriveRequest
            {
                DriveId = driveId
            }).ConfigureAwait(false)).Drive;

            DriveInfo driveInfo = new DriveInfo
            {
                Drive = drive,
                Username = person.Username
            };

            await Navigation.PushAsync(new DriveInfoPage()
            {
                BindingContext = driveInfo
            }).ConfigureAwait(false);
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {

        }

        async void OnFriendsListButtonClicked(object sender, EventArgs args)
        {
            var person = (Person)BindingContext;
            var friends = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = person.Username
            }).ConfigureAwait(false)).Friends.Select(o => (Friend)o);

            await Navigation.PushAsync(new FriendsPage()
            {
                BindingContext = friends
            }).ConfigureAwait(false);
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

            await Navigation.PushModalAsync(new NewDriveDestinationPage()
            {
                BindingContext = drive
            }).ConfigureAwait(false);
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No").ConfigureAwait(false);
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