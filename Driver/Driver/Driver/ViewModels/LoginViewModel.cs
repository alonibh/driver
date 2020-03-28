using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly DialogService _dialogService;
        private readonly RemoteDbHelper _dbHelper;
        private LoginRequest _loginRequest = new LoginRequest();

        public ICommand OnSigninButtonClicked => new Command(async () => await Signin());
        public ICommand OnSignupButtonClicked => new Command(async () => await Signup());
        public LoginRequest LoginRequest
        {
            get { return _loginRequest; }
            set
            {
                _loginRequest = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dialogService = new DialogService();
            _dbHelper = new RemoteDbHelper();
        }

        async Task Signin()
        {
            LoginResponse loginResponse = await _dbHelper.Login(new LoginRequest
            {
                Username = LoginRequest.Username,
                Password = LoginRequest.Password
            });



            if (!loginResponse.Success)
            {
                await _dialogService.ShowMessage("Error", "Wrong user name or password", "OK");
            }
            else
            {
                Application.Current.Properties["username"] = LoginRequest.Username;
                Application.Current.Properties["token"] = loginResponse.Token;
                _dbHelper.SetToken(loginResponse.Token);

                GetPersonResponse getPersonResponse = await _dbHelper.GetPerson(new GetPersonRequest
                {
                    Username = LoginRequest.Username
                });

                GetPersonDrivesResponse getPersonDrivesResponse = await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = LoginRequest.Username
                });

                Person person = new Person
                {
                    Username = getPersonResponse.Person.Username,
                    Address = getPersonResponse.Person.Address,
                    FirstName = getPersonResponse.Person.FirstName,
                    LastName = getPersonResponse.Person.LastName,
                    Email = getPersonResponse.Person.Email,
                    Drives = getPersonDrivesResponse.Drives.Select(o => (Drive)o).ToList(),
                    Friends = new List<Friend>()
                };
                MainPage mainPage = new MainPage(person);

                var currPage = _navigation.NavigationStack[0];
                _navigation.InsertPageBefore(mainPage, currPage);
                await _navigation.PopAsync();
            }
        }

        async Task Signup()
        {
            await _navigation.PushAsync(new SignUpPage());
        }
    }
}