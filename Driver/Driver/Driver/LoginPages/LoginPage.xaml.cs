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
            LoginResponse loginResponse = null;
            try
            {
                loginResponse = await App.Database.Login(new LoginRequest
                {
                    Username = usernameEntry.Text,
                    Password = passwordEntry.Text
                });
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!loginResponse.Success)
            {
                await DisplayAlert("Error", "Wrong user name or password", "OK");
            }
            else
            {
                Application.Current.Properties["username"] = usernameEntry.Text;
                Application.Current.Properties["token"] = loginResponse.Token;
                App.Database.SetToken(loginResponse.Token);

                GetPersonResponse getPersonResponse;
                try
                {
                    getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                    {
                        Username = usernameEntry.Text
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
                        Username = usernameEntry.Text
                    }));
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                    return;
                }

                MainPage mainPage = new MainPage()
                {
                    BindingContext = new Person
                    {
                        Username = getPersonResponse.Person.Username,
                        Address = getPersonResponse.Person.Address,
                        FirstName = getPersonResponse.Person.FirstName,
                        LastName = getPersonResponse.Person.LastName,
                        Email = getPersonResponse.Person.Email,
                        Drives = getPersonDrivesResponse.Drives.Select(o => (Drive)o).ToList(),
                        Friends = new List<Friend>()
                    }
                };

                Navigation.InsertPageBefore(mainPage, this);
                await Navigation.PopAsync();
            }
        }

        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}