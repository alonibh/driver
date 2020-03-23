using Driver.Models;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace Driver.MainPages
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();

            var acceptedFriends = friends.Where(o => o.Status == FriendRequestStatus.Accepted);
            var pendingFriends = friends.Where(o => o.Status == FriendRequestStatus.Pending);
            FriendsPage approvedFriendsPage = new FriendsPage(acceptedFriends, username);
            approvedFriendsPage.Title = "Friends";
            FriendsPage pendingFriendRequestsPage = new FriendsPage(pendingFriends, username);
            pendingFriendRequestsPage.Title = "Pending Requests";

            Children.Add(approvedFriendsPage);
            Children.Add(pendingFriendRequestsPage);
        }
    }
}