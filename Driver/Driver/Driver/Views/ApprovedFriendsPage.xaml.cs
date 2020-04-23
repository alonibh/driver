using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class ApprovedFriendsPage : ContentPage
    {
        private readonly ApprovedFriendsViewModel _viewModel;

        public ApprovedFriendsPage(IEnumerable<Friend> friends, IEnumerable<Drive> drives, string username)
        {
            InitializeComponent();
            _viewModel = new ApprovedFriendsViewModel(friends, drives, username);
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