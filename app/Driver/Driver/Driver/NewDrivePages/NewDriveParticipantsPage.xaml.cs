using Driver.DB.DBO;
using Driver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Driver.NewDrivePages
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        public NewDriveParticipantsPage(int userId)
        {
            InitializeComponent();
            string friendsStr = App.Database.GetUserFriends(userId);
            List<Friend> friends = new List<Friend>()
            {
                new Friend
                {
                    Address = "Usha 15",
                    FirstName = "Dani",
                    LastName = "Bar",
                },
                new Friend
                {
                    Address = "Usha 14",
                    FirstName = "Roei",
                    LastName = "Jac",

                }
            };
            if (friendsStr != string.Empty)
                friends = JsonConvert.DeserializeObject<List<FriendDbo>>(friendsStr).Select(o => (Friend)o).ToList();
            ObservableCollection<Friend> observableFriends = new ObservableCollection<Friend>(friends);

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
                    Id = friend.Id,
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