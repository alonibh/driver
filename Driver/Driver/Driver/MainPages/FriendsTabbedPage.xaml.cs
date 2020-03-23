using Driver.Models;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Driver.MainPages
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();

            ApprovedFriendsPage approvedFriendsPage = new ApprovedFriendsPage(friends, username);
            approvedFriendsPage.Title = "Friends";
            PendingFriendsPage pendingFriendRequestsPage = new PendingFriendsPage(friends, username);
            pendingFriendRequestsPage.Title = "Pending Requests";

            Children.Add(approvedFriendsPage);
            Children.Add(pendingFriendRequestsPage);
        }
    }
}