using Driver.API;
using Driver.Helpers;
using MvvmHelpers;
using Plugin.Toast;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private SignupRequest _signupRequest = new SignupRequest();
        private DialogService _dialogService;

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
            _dialogService = new DialogService();
        }

        async Task Signup()
        {
            SignupResponse signupResponse;
            try
            {
                signupResponse = await App.Database.SignUp(new SignupRequest
                {
                    Username = SignupRequest.Username,
                    Password = SignupRequest.Password,
                    FirstName = SignupRequest.FirstName,
                    LastName = SignupRequest.LastName,
                    Address = SignupRequest.Address,
                    Email = SignupRequest.Email
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!signupResponse.Success)
            {
                await _dialogService.ShowMessage("Error", "Unable to sign user", "OK");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastSuccess("Success!");
                await _navigation.PopToRootAsync();
            }
        }
    }
}