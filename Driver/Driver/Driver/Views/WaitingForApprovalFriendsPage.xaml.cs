using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class WaitingForApprovalFriendsPage : ContentPage
    {
        private readonly WaitingForApprovalFriendsViewModel _viewModel;

        public WaitingForApprovalFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new WaitingForApprovalFriendsViewModel(new ObservableCollection<Friend>(friends), username);
            BindingContext = _viewModel;
            WaitingForApprovalFriendsListView.ItemTapped += _viewModel.OnWaitingForApprovalFriendTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.SubscribePoppingEvent();
            _viewModel.ReloadFriends();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.UnsubscribePoppingEvent();
        }
    }
}