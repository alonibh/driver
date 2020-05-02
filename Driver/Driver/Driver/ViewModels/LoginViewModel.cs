using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        private LoginRequest _loginRequest;
        public LoginRequest LoginRequest
        {
            get { return _loginRequest; }
            set
            {
                _loginRequest = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnSigninButtonClicked => new Command(async () => await Signin());
        public ICommand OnSignupButtonClicked => new Command(async () => await Signup());

        public LoginViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dialogService = DependencyService.Get<IDialogService>();
            _dbHelper = DependencyService.Get<IDbHelper>();

            LoginRequest = new LoginRequest();
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
                await _dialogService.ShowMessage("Wrong user name or password", "Error", "OK", null);
            }
            else
            {
                Application.Current.Properties["username"] = LoginRequest.Username;
                Application.Current.Properties["token"] = loginResponse.Token;
                _dbHelper.SetToken(loginResponse.Token);

                var personDbo = (await _dbHelper.GetPerson(new GetPersonRequest
                {
                    Username = LoginRequest.Username
                })).Person;

                var drives = (await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = LoginRequest.Username
                })).Drives;

                var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = LoginRequest.Username
                })).Friends;

                Person person = new Person
                {
                    Username = personDbo.Username,
                    Address = personDbo.Address,
                    FirstName = personDbo.FirstName,
                    LastName = personDbo.LastName,
                    Email = personDbo.Email,
                    Drives = drives.Select(o => (Drive)o).ToList(),
                    Friends = friends.Select(o => (Friend)o).ToList()
                };
                MainPage mainPage = new MainPage(person);

                Application.Current.MainPage = mainPage;
            }
        }

        async Task Signup()
        {
            await _navigation.PushAsync(new SignUpPage());
        }
    }
}