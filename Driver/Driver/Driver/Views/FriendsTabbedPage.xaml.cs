using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage(IEnumerable<Friend> friends, IEnumerable<Drive> drives, string username)
        {
            InitializeComponent();

            var bindingContext = new FriendsTabbedViewModel(Navigation);
            BindingContext = bindingContext;

            ToolbarItem item = new ToolbarItem
            {
                IconImageSource = ImageSource.FromFile("find.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                Command = bindingContext.OnSearchPersonToolbarItemClicked
            };
            ToolbarItems.Add(item);

            ApprovedFriendsPage approvedFriendsPage = new ApprovedFriendsPage(friends, drives, username)
            {
                Title = "Friends"
            };
            WaitingForApprovalFriendsPage waitingForApprovalFriendRequestsPage = new WaitingForApprovalFriendsPage(friends, username)
            {
                Title = "Requests"
            };

            Children.Add(approvedFriendsPage);
            Children.Add(waitingForApprovalFriendRequestsPage);
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopToRootAsync();
            });

            return true;
        }
    }
}