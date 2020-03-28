using Driver.Models;
using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class DriveInfoPage : ContentPage
    {
        public DriveInfoPage(Drive drive, string username)
        {
            InitializeComponent();
            BindingContext = new DriveInfoViewModel(drive, username, Navigation);
        }
    }
}