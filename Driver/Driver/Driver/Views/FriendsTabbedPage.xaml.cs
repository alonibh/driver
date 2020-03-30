using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Driver.Views
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();

            BindingContext = new FriendsTabbedViewModel(Navigation);

            ApprovedFriendsPage approvedFriendsPage = new ApprovedFriendsPage(friends, username)
            {
                Title = "Friends"
            };
            WaitingForApprovalFriendsFriendsPage waitingForApprovalFriendRequestsPage = new WaitingForApprovalFriendsFriendsPage(friends, username)
            {
                Title = "Waiting For Approval Requests"
            };

            Children.Add(approvedFriendsPage);
            Children.Add(waitingForApprovalFriendRequestsPage);
        }
    }
}