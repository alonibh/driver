using Driver.Models;
using System;
using System.Linq;
using Xamarin.Forms;

namespace Driver.MainPages
{
    public partial class DriveInfoPage : ContentPage
    {
        public DriveInfoPage()
        {
            InitializeComponent();
        }
        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Delete Drive", "Are you sure you want to delete this drive?", "Yes", "No");
            if (answer)
            {
                var driveInfo = (DriveInfo)BindingContext;
                await App.Database.DeleteDrive(driveInfo.Drive.Id);

                var mainPage = Navigation.NavigationStack[0];
                var bindingContext = (User)mainPage.BindingContext;
                bindingContext.Drives.RemoveAll(o => o.Id == driveInfo.Drive.Id);

                var existingPages = Navigation.NavigationStack.ToList();

                await Navigation.PushAsync(new MainPage
                {
                    BindingContext = bindingContext
                });

                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
            }
        }

    }
}