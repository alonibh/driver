using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class WaitingForApprovalFriendsPage : ContentPage
    {
        private readonly WaitingForApprovalFriendsViewModel _viewModel;

        public WaitingForApprovalFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new WaitingForApprovalFriendsViewModel(friends, username);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.ReloadFriends();
        }
    }
}