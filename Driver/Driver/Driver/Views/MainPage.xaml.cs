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

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result)
                {
                    DependencyService.Get<IAndroidMethods>().CloseApp(); // TODO - Improve
                }
            });

            return true;
        }
    }

    public interface IAndroidMethods
    {
        void CloseApp();
    }
}