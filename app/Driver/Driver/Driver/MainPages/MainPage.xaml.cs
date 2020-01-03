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
            await Navigation.PushAsync(new NewDriveNamePage()
            {
                BindingContext = new Drive
                {
                    Date = DateTime.Now,
                    Driver = new DriveParticipant
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Id = user.Id,
                    }
                }
            });
        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IAndroidMethods>().CloseApp();

            return base.OnBackButtonPressed();
        }
    }
    public interface IAndroidMethods
    {
        void CloseApp();
    }
}