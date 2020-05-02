using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class WaitingForApprovalFriendsPage : ContentPage
    {
        private readonly WaitingForApprovalFriendsViewModel _viewModel;

        public WaitingForApprovalFriendsPage()
        {
            InitializeComponent();
            _viewModel = new WaitingForApprovalFriendsViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.ReloadFriends();
        }
    }
}