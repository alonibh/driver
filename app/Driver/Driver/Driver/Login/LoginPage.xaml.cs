using Driver.MainPages;
using Driver.Models;
using System;
using System.Collections.Generic;
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
                        Image = user.Image,
                        Drives = new List<Drive>
                        {
                            new Drive
                            {
                                Name="Drive name",
                                Date= DateTime.Now,
                                Destination = "DEST",
                                Driver = new DriveParticipant
                                {
                                    FirstName = "First name",
                                    LastName = "Last name"
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "First name 1",
                                    LastName = "Last name"
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "First name 2",
                                    LastName = "Last name"
                                }
                                }
                            }, new Drive
                            {
                                Name="Drive name",
                                Date= DateTime.Now,
                                Destination = "DEST",
                                Driver = new DriveParticipant
                                {
                                    FirstName = "First name",
                                    LastName = "Last name"
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "First name 1",
                                    LastName = "Last name"
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "First name 2",
                                    LastName = "Last name"
                                }
                                }
                            }, new Drive
                            {
                                Name="Drive name",
                                Date= DateTime.Now,
                                Destination = "DEST",
                                Driver = new DriveParticipant
                                {
                                    FirstName = "First name",
                                    LastName = "Last name"
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "First name 1",
                                    LastName = "Last name"
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "First name 2",
                                    LastName = "Last name"
                                }
                                }
                            }, new Drive
                            {
                                Name="Drive name",
                                Date= DateTime.Now,
                                Destination = "DEST",
                                Driver = new DriveParticipant
                                {
                                    FirstName = "First name",
                                    LastName = "Last name"
                                },
                                Participants = new List<DriveParticipant>
                                {
                                    new DriveParticipant
                                {
                                    FirstName = "First name 1",
                                    LastName = "Last name"
                                },
                                    new DriveParticipant
                                {
                                    FirstName = "First name 2",
                                    LastName = "Last name"
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