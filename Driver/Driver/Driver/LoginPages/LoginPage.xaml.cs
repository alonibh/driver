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
            }).ConfigureAwait(false);

            if (!loginResponse.Success)
            {
                await DisplayAlert("Error", "Wrong user name or password", "OK").ConfigureAwait(false);
            }
            else
            {
                var person = (await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = usernameEntry.Text
                }).ConfigureAwait(false)).Person;

                var drives = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = usernameEntry.Text
                }).ConfigureAwait(false)).Drives;

                MainPage mainPage = new MainPage()
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
                };

                Navigation.InsertPageBefore(mainPage, this);
                await Navigation.PopAsync().ConfigureAwait(false);
            }
        }

        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignUpPage()).ConfigureAwait(false);
        }
    }
}