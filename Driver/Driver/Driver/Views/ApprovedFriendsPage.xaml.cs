using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class ApprovedFriendsPage : ContentPage
    {
        private readonly ApprovedFriendsViewModel _viewModel;

        public ApprovedFriendsPage()
        {
            InitializeComponent();
            _viewModel = new ApprovedFriendsViewModel();
            BindingContext = _viewModel;
            approvedFriendsCollectionView.SelectionChanged += _viewModel.OnApprovedFriendTapped;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.SubscribePoppingEvent();
            await _viewModel.ReloadFriends();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.UnsubscribePoppingEvent();
        }
    }
}