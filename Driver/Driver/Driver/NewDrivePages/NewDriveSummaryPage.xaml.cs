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

            string driver = JsonConvert.SerializeObject(drive.Driver);
            var participants = drive.Participants;
            participants.Add(drive.Driver);
            AddDriveResponse addDriveResponse = null;
            try
            {
                addDriveResponse = await App.Database.AddDrive(new AddDriveRequest
                {
                    Dest = drive.Destination,
                    Date = drive.Date,
                    Driver = drive.Driver.Username,
                    Participants = participants.Select(o => o.Username).ToList()
                });
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!addDriveResponse.Success)
            {
                await DisplayAlert("Error", "Unable to add drive", "OK");
                return;
            }

            GetPersonResponse getPersonResponse;
            try
            {
                getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = drive.Driver.Username
                }));
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            GetPersonDrivesResponse getPersonDrivesResponse;
            try
            {
                getPersonDrivesResponse = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = drive.Driver.Username
                }));
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            var bindingContext = new Person
            {
                Username = getPersonResponse.Person.Username,
                Address = getPersonResponse.Person.Address,
                FirstName = getPersonResponse.Person.FirstName,
                LastName = getPersonResponse.Person.LastName,
                Email = getPersonResponse.Person.Email,
                Drives = getPersonDrivesResponse.Drives.Select(o => (Drive)o).ToList(),
                Friends = new List<Friend>()
            };

            MainPage mainPage = new MainPage
            {
                BindingContext = bindingContext
            };

            var existingPages = Navigation.NavigationStack.ToList();
            await Navigation.PushAsync(mainPage);
            foreach (var page in existingPages)
                Navigation.RemovePage(page);

            //Navigation.NavigationStack[0].BindingContext = bindingContext;
            //await Navigation.PopToRootAsync(true);
        }
    }
}