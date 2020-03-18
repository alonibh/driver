using Driver.API;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveDestinationPage : ContentPage
    {
        public NewDriveDestinationPage()
        {
            InitializeComponent();
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            drive.Destination = driveDestEntry.Text;

            var person = (await App.Database.GetPerson(new GetPersonRequest
            {
                Username = drive.Driver.Username
            })).Person;

            List<Person> friends = new List<Person>();
            if (person.FriendsUsernames != null)
            {
                List<string> friendsUsernames = JsonConvert.DeserializeObject<List<string>>(person.FriendsUsernames);

                foreach (var frientUsername in friendsUsernames)
                {
                    var friend = (await App.Database.GetPerson(new GetPersonRequest
                    {
                        Username = frientUsername
                    })).Person;

                    friends.Add(friend);
                }
            }

            ObservableCollection<Person> observableFriends = new ObservableCollection<Person>(friends);

            await Navigation.PushAsync(new NewDriveParticipantsPage(observableFriends)
            {
                BindingContext = drive
            });
        }
    }
}