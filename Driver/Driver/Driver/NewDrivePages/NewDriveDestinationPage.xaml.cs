﻿using Driver.API;
using Driver.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveDestinationPage : ContentPage
    {
        public NewDriveDestinationPage()
        {
            InitializeComponent();
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            drive.Destination = driveDestEntry.Text;

            var friends = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
            {
                Username = drive.Driver.Username
            }).ConfigureAwait(false)).Friends.Select(o => (Friend)o);

            var observableFriends = new ObservableCollection<Friend>(friends);

            await Navigation.PushModalAsync(new NewDriveParticipantsPage(observableFriends)
            {
                BindingContext = drive
            }).ConfigureAwait(false);
        }
    }
}