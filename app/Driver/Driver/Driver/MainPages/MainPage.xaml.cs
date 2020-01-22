using Driver.Models;
using Driver.NewDrivePages;
using System;
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
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            var user = (User)BindingContext;
            var drive = e.Item as Drive;
            DriveInfo driveInfo = new DriveInfo();

            if (user.Id == drive.Driver.Id)
                driveInfo.IsUserDriver = true;
            else
                driveInfo.IsUserDriver = false;

            driveInfo.Drive = drive;
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
            await Navigation.PushAsync(new FriendsPage()
            {
                BindingContext = BindingContext
            });
        }

        async void OnNewDriveButtonClicked(object sender, EventArgs args)
        {
            var user = (User)BindingContext;
            var drive = new Drive
            {
                Date = DateTime.Now,
                Driver = new DriveParticipant
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                }
            };
            await Navigation.PushAsync(new NewDriveNamePage()
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