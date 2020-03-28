using Driver.API;
using Driver.Helpers;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using Plugin.Toast;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private SignupRequest _signupRequest = new SignupRequest();
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        public ICommand OnSignupButtonClicked => new Command(async () => await Signup());
        public SignupRequest SignupRequest
        {
            get { return _signupRequest; }
            set
            {
                _signupRequest = value;
                OnPropertyChanged();
            }
        }

        public SignupViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dialogService = DependencyService.Get<IDialogService>();
            _dbHelper = DependencyService.Get<IDbHelper>();
        }

        async Task Signup()
        {
            SignupResponse signupResponse = await _dbHelper.SignUp(new SignupRequest
            {
                Username = SignupRequest.Username,
                Password = SignupRequest.Password,
                FirstName = SignupRequest.FirstName,
                LastName = SignupRequest.LastName,
                Address = SignupRequest.Address,
                Email = SignupRequest.Email
            });

            if (!signupResponse.Success)
            {
                await _dialogService.ShowMessage("Unable to sign user", "Error", "OK", null);
            }
            else
            {
                CrossToastPopUp.Current.ShowToastSuccess("Success!");
                await _navigation.PopToRootAsync();
            }
        }
    }
}