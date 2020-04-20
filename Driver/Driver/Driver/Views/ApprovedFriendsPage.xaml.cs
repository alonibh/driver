using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class ApprovedFriendsPage : ContentPage
    {
        private readonly ApprovedFriendsViewModel _viewModel;

        public ApprovedFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new ApprovedFriendsViewModel(new ObservableCollection<Friend>(friends), username);
            BindingContext = _viewModel;
            approvedFriendsCollectionView.SelectionChanged += _viewModel.OnApprovedFriendTapped;
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