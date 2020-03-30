using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class WaitingForApprovalFriendsFriendsPage : ContentPage
    {
        private readonly FriendsViewModel _viewModel;

        public WaitingForApprovalFriendsFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new FriendsViewModel(new ObservableCollection<Friend>(friends), username);
            BindingContext = _viewModel;
            WaitingForApprovalFriendsListView.ItemTapped += _viewModel.OnWaitingForApprovalFriendTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.ReloadFriends();
        }
    }
}