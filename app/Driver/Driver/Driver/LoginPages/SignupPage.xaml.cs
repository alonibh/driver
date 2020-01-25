using Driver.DB.DBO;
using Driver.MainPages;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Driver.LoginPages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            if (App.Database.IsUsernameTaken(usernameEntry.Text))
            {
                await DisplayAlert("Error", "User name already taken, please choose a different one", "OK");
            }
            else
            {
                int userId = App.Database.SignUpUser(usernameEntry.Text, passwordEntry.Text, firstNameEntry.Text, lastNameEntry.Text, addressEntry.Text, null);
                if (userId == -1)
                    await DisplayAlert("Error", "Unable to add user", "OK");
                else
                {
                    var user = new User
                    {
                        Id = userId,
                        FirstName = firstNameEntry.Text,
                        LastName = lastNameEntry.Text,
                        Address = addressEntry.Text,
                        Image = null,
                        Drives = new List<Drive>(),
                        //Friends = new List<Friend>() // TODO - When supporting specific friends for each user
                    };

                    //TODO - Remove when supporting specific friends for each user
                    string friendsStr = App.Database.GetUserFriends(user.Id);
                    List<Friend> friends = new List<Friend>();
                    if (friendsStr != string.Empty)
                        friends = JsonConvert.DeserializeObject<List<FriendDbo>>(friendsStr).Select(o => (Friend)o).ToList();
                    user.Friends = friends;

                    var prevPages = new List<Page>(Navigation.NavigationStack);

                    await Navigation.PushAsync(new MainPage()
                    {
                        BindingContext = user
                    });

                    foreach(Page page in prevPages)
                        Navigation.RemovePage(page);
                }
            }
        }
    }
}