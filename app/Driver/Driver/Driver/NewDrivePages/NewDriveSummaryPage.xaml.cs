using Driver.DB.DBO;
using Driver.MainPages;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveSummaryPage : ContentPage
    {
        public NewDriveSummaryPage()
        {
            InitializeComponent();

        }
        async void OnAddDriveButtonClicked(object sender, EventArgs args)
        {
            var drive = (Drive)BindingContext;
            List<DriveParticipantDbo> participants = drive.Participants.Select(o => (DriveParticipantDbo)o).ToList();
            string participantsStr = JsonConvert.SerializeObject(participants);

            DriveParticipantDbo driver = drive.Driver;
            string driverStr = JsonConvert.SerializeObject(driver);

            await App.Database.AddDrive(drive.Destination, drive.Date, participantsStr, driverStr);

            var mainPage = Navigation.NavigationStack[0];
            var bindingContext = (User)mainPage.BindingContext;
            bindingContext.Drives.Add(drive);
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