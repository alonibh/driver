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

        async void onDriveTapped(object sender, ItemTappedEventArgs e)
        {
            var person = (Person)BindingContext;

            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            int driveId = (e.Item as Drive).Id;
            var drive = (await App.Database.GetDrive(new GetDriveRequest
            {
                DriveId = driveId
            })).Drive;

            DriveInfo driveInfo = new DriveInfo
            {
                Drive = drive,
                Username = person.Username
            };

            await Navigation.PushAsync(new DriveInfoPage()
            {
                BindingContext = driveInfo
            });
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
            })).Friends.Select(o => (Friend)o);

            await Navigation.PushAsync(new FriendsPage()
            {
                BindingContext = friends
            });
        }

        async void OnNewDriveButtonClicked(object sender, EventArgs args)
        {
            var person = (Person)BindingContext;
            var drive = new Drive
            {
                Date = DateTime.Now,
                Driver = person
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
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
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