using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveSummaryViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private DialogService _dialogService;

        public ICommand OnAddDriveButtonClicked => new Command(async () => await AddDrive());
        public Drive Drive { get; set; }

        public NewDriveSummaryViewModel(Drive drive, INavigation navigation)
        {
            Drive = drive;
            _navigation = navigation;
            _dialogService = new DialogService();
        }

        async Task AddDrive()
        {
            string driver = JsonConvert.SerializeObject(Drive.Driver);
            var participants = Drive.Participants;
            participants.Add(Drive.Driver);
            AddDriveResponse addDriveResponse = null;
            try
            {
                addDriveResponse = await App.Database.AddDrive(new AddDriveRequest
                {
                    Dest = Drive.Destination,
                    Date = Drive.Date,
                    Driver = Drive.Driver.Username,
                    Participants = participants.Select(o => o.Username).ToList()
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            if (!addDriveResponse.Success)
            {
                await _dialogService.ShowMessage("Error", "Unable to add drive", "OK");
                return;
            }

            GetPersonResponse getPersonResponse;
            try
            {
                getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = Drive.Driver.Username
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
                    Username = Drive.Driver.Username
                }));
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                return;
            }

            var person = new Person
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

            var rootPage = _navigation.NavigationStack[0];
            _navigation.InsertPageBefore(mainPage, rootPage);
            await _navigation.PopToRootAsync();
        }
    }
}