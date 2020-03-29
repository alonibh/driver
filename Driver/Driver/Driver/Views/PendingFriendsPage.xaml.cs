using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class PendingFriendsPage : ContentPage
    {
        private FriendsViewModel _viewModel;

        public PendingFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            _viewModel = new FriendsViewModel(new ObservableCollection<Friend>(friends), username);
            BindingContext = _viewModel;
            pendingFriendsListView.ItemTapped += _viewModel.OnPendingFriendTapped;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.ReloadFriends();
        }
    }
}