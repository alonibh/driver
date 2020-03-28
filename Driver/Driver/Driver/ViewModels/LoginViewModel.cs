using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private DialogService _dialogService;
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
        }

        async Task Signin()
        {
            LoginResponse loginResponse = null;
            try
            {
                loginResponse = await App.Database.Login(new LoginRequest
                {
                    Username = LoginRequest.Username,
                    Password = LoginRequest.Password
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!loginResponse.Success)
            {
                await _dialogService.ShowMessage("Error", "Wrong user name or password", "OK");
            }
            else
            {
                Application.Current.Properties["username"] = LoginRequest.Username;
                Application.Current.Properties["token"] = loginResponse.Token;
                App.Database.SetToken(loginResponse.Token);

                GetPersonResponse getPersonResponse;
                try
                {
                    getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                    {
                        Username = LoginRequest.Username
                    }));
                }
                catch (Exception e)
                {
                    await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                    return;
                }

                GetPersonDrivesResponse getPersonDrivesResponse;
                try
                {
                    getPersonDrivesResponse = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                    {
                        Username = LoginRequest.Username
                    }));
                }
                catch (Exception e)
                {
                    await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                    return;
                }
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