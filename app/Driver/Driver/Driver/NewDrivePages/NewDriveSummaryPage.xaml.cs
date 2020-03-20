using Driver.API;
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
            string participants = JsonConvert.SerializeObject(drive.Participants);

            string driver = JsonConvert.SerializeObject(drive.Driver);

            await App.Database.AddDrive(new AddDriveRequest
            {
                Destination = drive.Destination,
                Date = drive.Date,
                Driver = drive.Driver.Username,
                Participants = drive.Participants.Select(o => o.Username).ToList()
            });

            var person = (await App.Database.GetPerson(new GetPersonRequest
            {
                Username = drive.Driver.Username
            })).Person;

            var drives = await App.Database.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = drive.Driver.Username
            });

            var bindingContext = new Person
            {
                Username = person.Username,
                Address = person.Address,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Drives = drives.Drives.Select(o => (Drive)o).ToList(),
                Friends = new List<Friend>()
            };

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