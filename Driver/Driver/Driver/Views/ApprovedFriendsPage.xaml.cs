using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class ApprovedFriendsPage : ContentPage
    {
        private readonly FriendsViewModel _viewModel;

        public ApprovedFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new FriendsViewModel(new ObservableCollection<Friend>(friends), username);
            BindingContext = _viewModel;
            approvedFriendsListView.ItemTapped += _viewModel.OnApprovedFriendTapped;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.ReloadFriends();
        }
    }
}