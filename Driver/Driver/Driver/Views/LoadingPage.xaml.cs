using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
            BindingContext = new LoadingViewModel();
        }
    }
}