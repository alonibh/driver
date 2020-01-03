using System;
using Driver.Models;
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
            if(answer)
            {
                var driveInfo = (DriveInfo) BindingContext;
                await App.Database.DeleteDrive(driveInfo.Drive.Id);
            }
        }

    }
}