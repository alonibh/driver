using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class FriendsTabbedPage : TabbedPage
    {
        public FriendsTabbedPage()
        {
            InitializeComponent();

            var bindingContext = new FriendsTabbedViewModel();
            BindingContext = bindingContext;

            ToolbarItem item = new ToolbarItem
            {
                IconImageSource = ImageSource.FromFile("find.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                Command = new Command(async () =>
                {
                    await Navigation.PushAsync(new SearchPersonPage());
                })
            };
            ToolbarItems.Add(item);

            ApprovedFriendsPage approvedFriendsPage = new ApprovedFriendsPage()
            {
                Title = "Friends",
                IconImageSource = ImageSource.FromFile("friendslist.png")
            };
            WaitingForApprovalFriendsPage waitingForApprovalFriendRequestsPage = new WaitingForApprovalFriendsPage()
            {
                Title = "Requests",
                IconImageSource = ImageSource.FromFile("friendrequest.png")
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