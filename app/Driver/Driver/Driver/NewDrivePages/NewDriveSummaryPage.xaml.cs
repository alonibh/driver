using Driver.DB.DBO;
using Driver.MainPages;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Driver.NewDrivePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDriveSummaryPage : ContentPage
    {
        public NewDriveSummaryPage()
        {
            InitializeComponent();
            Title = "Summery";

        }
        async void OnAddDriveButtonClicked(object sender, EventArgs args)
        {
            var drive = (Drive)BindingContext;
            List<DriveParticipantDbo> participants= drive.Participants.Select(o => (DriveParticipantDbo)o).ToList();
            string participantsStr = JsonConvert.SerializeObject(participants);

            DriveParticipantDbo driver = drive.Driver;
            string driverStr = JsonConvert.SerializeObject(driver);

            await App.Database.AddDrive(drive.Name,drive.Destination,drive.Date, participantsStr, driverStr);

            await Navigation.PopToRootAsync(true);
        }

    }
}