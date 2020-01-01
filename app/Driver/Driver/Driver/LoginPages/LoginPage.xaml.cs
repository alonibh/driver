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

                await Navigation.PushAsync(new MainPage()
                {
                    BindingContext = new User
                    {
                        ID = user.ID,
                        Address = user.Address,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Drives = drives,
                    }
                });
            }
        }
        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}