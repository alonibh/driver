using Driver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.NewDrivePages
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        public NewDriveParticipantsPage(ObservableCollection<Person> observableFriends)
        {
            InitializeComponent();
            friendsCollectionView.BindingContext = observableFriends;
        }

        async void OnNextButtonClicked(object sender, EventArgs e)
        {
            var drive = (Drive)BindingContext;
            var friends = friendsCollectionView.SelectedItems;
            List<Person> participants = new List<Person>();
            foreach (Person friend in friends)
            {
                participants.Add(new Person
                {
                    Username = friend.Username,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName
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