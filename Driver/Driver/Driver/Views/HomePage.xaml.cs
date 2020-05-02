using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = new HomePageViewModel(Navigation);
            BindingContext = viewModel;
        }
    }
}