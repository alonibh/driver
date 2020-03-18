using Driver.API;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                bool isSeccessful = await App.Database.DeleteDrive(new DeleteDriveRequest
                {
                    DriveId = driveInfo.Drive.Id
                });

                if (!isSeccessful)
                    await DisplayAlert("Error", "Failed to delete drive", "OK");
                else
                {
                    var mainPage = Navigation.NavigationStack[0];

                    var person = await App.Database.GetPerson(new GetPersonRequest
                    {
                        Username = driveInfo.Username
                    });

                    var drives = await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                    {
                        Username = driveInfo.Username
                    });

                    List<string> friendsUsernames = JsonConvert.DeserializeObject<List<string>>(person.FriendsUsernames);
                    List<Person> friends = new List<Person>();

                    foreach (var frientUsername in friendsUsernames)
                    {
                        var friend = await App.Database.GetPerson(new GetPersonRequest
                        {
                            Username = frientUsername
                        });
                        friends.Add(friend);
                    }

                    var existingPages = Navigation.NavigationStack.ToList();

                    await Navigation.PushAsync(new MainPage()
                    {
                        BindingContext = new Person
                        {
                            Username = person.Username,
                            Address = person.Address,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Email = person.Email,
                            Drives = drives.Select(o => (Drive)o).ToList(),
                            Friends = friends
                        }
                    });

                    foreach (var page in existingPages)
                    {
                        Navigation.RemovePage(page);
                    }
                }
            }
        }

    }
}