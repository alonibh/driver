using Driver.DB.DBO;
using Driver.MainPages;
using Driver.Models;
using System;
using Xamarin.Forms;

namespace Driver.Login
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }
        async void OnSignupButtonClicked(object sender, EventArgs args)
        {
            if (App.Database.IsUsernameTaken(usernameEntry.Text))
            {
                await DisplayAlert("Error", "User name already taken, plase choose a different one", "OK");
            }
            else
            {
                int rowsAdded = await App.Database.SignupUser(usernameEntry.Text, passwordEntry.Text, firstNameEntry.Text, lastNameEntry.Text, addressEntry.Text, null);
                if (rowsAdded == 0)
                    await DisplayAlert("Error", "Unable to add user", "OK");
                else
                {
                    await Navigation.PushAsync(new MainPage()
                    {
                        BindingContext = new User
                        {
                            FirstName = firstNameEntry.Text,
                            LastName = lastNameEntry.Text,
                            Address = addressEntry.Text,
                            Image = null
                        }
                    });
                }
            }
        }
    }
}