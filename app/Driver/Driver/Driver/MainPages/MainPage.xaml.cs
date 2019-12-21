using Driver.Models;
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
            var drive = e.Item as Drive;
            await Navigation.PushAsync(new DriveInfoPage()
            {
                BindingContext = drive
            });
        }

        async void OnSettingsButtonClicked(object sender, EventArgs args)
        {

        }

        async void OnFriendsListButtonClicked(object sender, EventArgs args)
        {

        }

        async void OnNewDriveButtonClicked(object sender, EventArgs args)
        {

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