using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class DrivesHistoryPage : ContentPage
    {
        public DrivesHistoryPage()
        {
            InitializeComponent();
            BindingContext = new DrivesHistoryViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Navigation.PopToRootAsync();
            });

            return true;
        }
    }
}