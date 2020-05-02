using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage()
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

            ApprovedFriendsPage approvedFriendsPage = new ApprovedFriendsPage()
            {
                Title = "Friends"
            };
            WaitingForApprovalFriendsPage waitingForApprovalFriendRequestsPage = new WaitingForApprovalFriendsPage()
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