using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            BindingContext = new SignupViewModel(Navigation);
        }
    }
}