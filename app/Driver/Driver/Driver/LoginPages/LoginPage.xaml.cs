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
            var user = await App.Database.GetUserProfile(usernameEntry.Text, passwordEntry.Text);
            if (user == null)
            {
                await DisplayAlert("Error", "Wrong user name or password", "OK");
            }
            else
            {
                var drivesIds = JsonConvert.DeserializeObject<List<int>>(user.DrivesIds);
                List<Drive> drives = new List<Drive>();
                if (drivesIds != null)
                {
                    drives = App.Database.GetDrives(drivesIds).Select(o => (Drive)o).ToList();
                }

                var friendsIds = JsonConvert.DeserializeObject<List<int>>(user.FriendsIds);
                List<Friend> friends = new List<Friend>();
                if (friendsIds != null)
                {
                    friends = App.Database.GetFriends(friendsIds).Select(o => (Friend)o).ToList();
                }
                var currPage = Navigation.NavigationStack[0];

                await Navigation.PushAsync(new MainPage()
                {
                    BindingContext = new User
                    {
                        Id = user.Id,
                        Address = user.Address,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Drives = drives,
                        Friends = /*friends*/ new List<Friend> // TODO remove template
                        {
                            new Friend
                            {
                                Id = 200,
                                Address = "Usha 15",
                                FirstName = "Dani",
                                LastName = "Bar",
                            },
                            new Friend
                            {
                                Id = 201,
                                Address = "Usha 14",
                                FirstName = "Roei",
                                LastName = "Jac",
                            }
                        }

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