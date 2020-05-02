using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        private readonly NewDriveParticipantsViewModel _viewModel;

        public NewDriveParticipantsPage()
        {
            InitializeComponent();
            _viewModel = new NewDriveParticipantsViewModel(Navigation);
            BindingContext = _viewModel;
            searchBar.TextChanged += _viewModel.OnTextChanged;
            friendsCollectionView.SelectionChanged += _viewModel.OnFriendTapped;
        }
    }
}