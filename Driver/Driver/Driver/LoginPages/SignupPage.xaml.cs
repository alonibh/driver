using Driver.API;
using Plugin.Toast;
using System;
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
            SignupResponse signupResponse;
            try
            {
                signupResponse = await App.Database.SignUp(new SignupRequest
                {
                    Username = usernameEntry.Text,
                    Password = passwordEntry.Text,
                    FirstName = firstNameEntry.Text,
                    LastName = lastNameEntry.Text,
                    Address = addressEntry.Text,
                    Email = emailEntry.Text
                });
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!signupResponse.Success)
                await DisplayAlert("Error", "Unable to sign user", "OK");

            else
            {
                CrossToastPopUp.Current.ShowToastMessage("Success!");
                await Navigation.PopToRootAsync();
            }
        }
    }
}