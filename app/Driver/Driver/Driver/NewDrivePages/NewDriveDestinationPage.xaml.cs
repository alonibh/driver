using Driver.Models;
using System;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveDestinationPage : ContentPage
    {
        public NewDriveDestinationPage()
        {
            InitializeComponent();
            Title = "New Drive";
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            drive.Destination = driveDestEntry.Text;
            await Navigation.PushAsync(new NewDriveParticipantsPage(drive.Driver.Id)
            {
                BindingContext = drive
            });
        }
    }
}