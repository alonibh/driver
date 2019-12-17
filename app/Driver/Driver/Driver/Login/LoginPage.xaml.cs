using Driver.Login;
using Driver.MainPages;
using Driver.Models;
using System;
using Xamarin.Forms;

namespace Driver.Login
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
                await Navigation.PushAsync(new MainPage()
                {
                    BindingContext = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        Image = null
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
