using Driver.API;
using Driver.MainPages;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Driver.LoginPages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        async void OnSigninButtonClicked(object sender, EventArgs args)
        {
            bool isSuccessful = await App.Database.Login(new LoginRequest
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            });
            if (!isSuccessful)
            {
                await DisplayAlert("Error", "Wrong user name or password", "OK");
            }
            else
            {
                var person = await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = usernameEntry.Text
                });

                var drives = await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = usernameEntry.Text
                });
                // TODO - When supporting specific friends for each user
                //var friendsIds = JsonConvert.DeserializeObject<List<int>>(user.FriendsIds);
                //List<Friend> friends = new List<Friend>();
                //if (friendsIds != null)
                //{
                //    friends = App.Database.GetFriends(friendsIds).Select(o => (Friend)o).ToList();
                //}

                //TODO - Remove when supporting specific friends for each user

                List<string> friendsUsernames = JsonConvert.DeserializeObject<List<string>>(person.FriendsUsernames);
                List<Person> friends = new List<Person>();
                if (friendsUsernames != null)
                {

                    foreach (var frientUsername in friendsUsernames)
                    {
                        var friend = await App.Database.GetPerson(new GetPersonRequest
                        {
                            Username = frientUsername
                        });
                        friends.Add(friend);
                    }
                }

                var currPage = Navigation.NavigationStack[0];

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

                Navigation.RemovePage(currPage);
            }
        }

        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}