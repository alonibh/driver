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
            bool isSuccessful = (await App.Database.SignUp(new SignupRequest
            {
                Username = usernameEntry.Text,
                Password = passwordEntry.Text,
                FirstName = firstNameEntry.Text,
                LastName = lastNameEntry.Text,
                Address = addressEntry.Text,
                Email = emailEntry.Text
            })).Success;

            if (!isSuccessful)
                await DisplayAlert("Error", "Unable to add user", "OK");

            else
            {
                CrossToastPopUp.Current.ShowToastMessage("Success!");
                await Navigation.PopToRootAsync();
            }
        }
    }
}