using Driver.MainPages;
using Driver.Models;
using System;
using System.Collections.Generic;
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
                await Navigation.PushAsync(new MainPage()
                {
                    BindingContext = new User
                    {
                        ID = 0,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        Image = user.Image,
                        Drives = new List<Drive>
                        {
                            new Drive
                            {
                                Name="Drive name1",
                                Date= DateTime.Now,
                                Destination = "DEST1",
                                Driver = new DriveParticipant
                                {
                                    FirstName = "Alon",
                                    LastName = "Ben Horin",
                                    ID = 0
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "Dani",
                                     LastName = "Mask",
                                    ID = 1
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "Yossi",
                                     LastName = "Ba",
                                    ID = 2
                                }
                                }
                            }, new Drive
                            {
                                Name="Drive name2",
                                Date= DateTime.Now,
                                Destination = "DEST2",
                               Driver = new DriveParticipant
                                {
                                    FirstName = "Dani",
                                    LastName = "Mask",
                                    ID = 1
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {

                                    FirstName = "Alon",
                                    LastName = "Ben Horin",
                                    ID = 0
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "Yossi",
                                     LastName = "Ba",
                                    ID = 2
                                }
                                }
                            }, new Drive
                            {
                                Name="Drive name3",
                                Date= DateTime.Now,
                                Destination = "DEST3",
                               Driver = new DriveParticipant
                                {
                                    FirstName = "Dani",
                                    LastName = "Mask",
                                    ID = 1
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "Alon",
                                    LastName = "Ben Horin",
                                    ID = 0
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "Yossi",
                                     LastName = "Ba",
                                    ID = 2
                                }
                                }
                            }
                        }
                        //user.Drives.Select(o => (Drive)o).ToList()
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