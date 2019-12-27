using Driver.Models;
using System;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveNamePage : ContentPage
    {
        public NewDriveNamePage()
        {
            InitializeComponent();
            Title = "New Drive";
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            drive.Name = driveNameEntry.Text;
            await Navigation.PushAsync(new NewDriveDestinationPage()
            {
                BindingContext = drive
            });
        }
    }
}