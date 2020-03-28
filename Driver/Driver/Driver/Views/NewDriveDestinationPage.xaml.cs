using Driver.Models;
using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveDestinationPage : ContentPage
    {
        public NewDriveDestinationPage(Drive drive)
        {
            InitializeComponent();
            BindingContext = new NewDriveDestinationViewModel(drive, Navigation);
        }
    }
}