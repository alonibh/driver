﻿using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using MvvmHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class NewDriveSummaryViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly DialogService _dialogService;
        private readonly RemoteDbHelper _dbHelper;

        public ICommand OnAddDriveButtonClicked => new Command(async () => await AddDrive());
        public Drive Drive { get; set; }

        public NewDriveSummaryViewModel(Drive drive, INavigation navigation)
        {
            Drive = drive;
            _navigation = navigation;
            _dialogService = new DialogService();
            _dbHelper = new RemoteDbHelper();
        }

        async Task AddDrive()
        {
            string driver = JsonConvert.SerializeObject(Drive.Driver);
            var participants = Drive.Participants;
            participants.Add(Drive.Driver);
            AddDriveResponse addDriveResponse = await _dbHelper.AddDrive(new AddDriveRequest
            {
                Dest = Drive.Destination,
                Date = Drive.Date,
                Driver = Drive.Driver.Username,
                Participants = participants.Select(o => o.Username).ToList()
            });

            if (!addDriveResponse.Success)
            {
                await _dialogService.ShowMessage("Error", "Unable to add drive", "OK");
                return;
            }

            GetPersonResponse getPersonResponse = await _dbHelper.GetPerson(new GetPersonRequest
            {
                Username = Drive.Driver.Username
            });

            GetPersonDrivesResponse getPersonDrivesResponse = await _dbHelper.GetPersonDrives(new GetPersonDrivesRequest
            {
                Username = Drive.Driver.Username
            });

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