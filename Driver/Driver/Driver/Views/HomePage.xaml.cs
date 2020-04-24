using Driver.Models;
using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(Person person)
        {
            InitializeComponent();
            var viewModel = new HomePageViewModel(person, Navigation);
            BindingContext = viewModel;
            //drivesListView.ItemTapped += viewModel.OnDriveTapped;
        }
    }
}