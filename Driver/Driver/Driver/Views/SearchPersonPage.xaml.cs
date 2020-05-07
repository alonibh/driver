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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await System.Threading.Tasks.Task.Delay(250);
                searchBar.Focus();
            });
        }
    }
}