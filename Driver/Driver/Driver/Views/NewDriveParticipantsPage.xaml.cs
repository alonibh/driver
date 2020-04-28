using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        private readonly NewDriveParticipantsViewModel _viewModel;

        public NewDriveParticipantsPage(Drive drive, IEnumerable<Friend> friends)
        {
            InitializeComponent();
            _viewModel = new NewDriveParticipantsViewModel(drive, friends, Navigation);
            BindingContext = _viewModel;
            searchBar.TextChanged += _viewModel.OnTextChanged;
            friendsCollectionView.SelectionChanged += _viewModel.OnFriendTapped;
        }
    }
}