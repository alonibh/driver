using Driver.API;
using Driver.MainPages;
using Driver.Models;
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
            var loginResponse = await App.Database.Login(new LoginRequest
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text
            });

            if (!loginResponse.Success)
            {
                await DisplayAlert("Error", "Wrong user name or password", "OK");
            }
            else
            {
                var person = (await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = usernameEntry.Text
                })).Person;

                var drives = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = usernameEntry.Text
                })).Drives;

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
                        Friends = new List<Friend>()
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