using Driver.Models;
using Driver.ViewModels;

using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveSummaryPage : ContentPage
    {
        public NewDriveSummaryPage(Drive drive)
        {
            InitializeComponent();
            BindingContext = new NewDriveSummaryViewModel(drive, Navigation);
        }
    }
}