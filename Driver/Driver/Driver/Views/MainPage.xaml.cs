using Driver.Models;
using Driver.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace Driver.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(Person person)
        {
            InitializeComponent();
            var viewModel = new MainPageViewModel(person, Navigation);
            BindingContext = viewModel;
            NavigationPage.SetHasBackButton(this, false);
            DataGridComponent.Init();
            drivesListView.ItemTapped += viewModel.OnDriveTapped;
        }
    }
}