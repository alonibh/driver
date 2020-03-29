using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class SearchPersonPage : ContentPage
    {
        public SearchPersonPage()
        {
            InitializeComponent();
            SearchPersonViewModel viewModel = new SearchPersonViewModel();
            BindingContext = viewModel;
            searchBar.TextChanged += viewModel.OnTextChanged;
            friendsListView.ItemTapped += viewModel.OnFriendTapped;
        }
    }
}