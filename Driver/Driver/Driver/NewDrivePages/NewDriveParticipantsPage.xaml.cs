using Driver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        public NewDriveParticipantsPage(ObservableCollection<Friend> observableFriends)
        {
            InitializeComponent();
            friendsCollectionView.BindingContext = observableFriends;
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            var friends = friendsCollectionView.SelectedItems;
            List<DriveParticipant> participants = new List<DriveParticipant>();
            foreach (Friend friend in friends)
            {
                participants.Add(new DriveParticipant
                {
                    Username = friend.Username,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    Address = friend.Address,
                });
            }

            drive.Participants = participants;
            await Navigation.PushAsync(new NewDriveSummaryPage()
            {
                BindingContext = drive
            });
        }
    }
}