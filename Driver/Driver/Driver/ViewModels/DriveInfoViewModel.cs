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
    public class DriveInfoViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private DialogService _dialogService;

        public ICommand OnDeleteDriveButtonClicked => new Command(async () => await DeleteDrive());
        public Drive Drive { get; set; }
        public string Username { get; }
        public bool IsUserDriver => Drive.Driver.Username == Username;
        public DriveInfoViewModel(Drive drive, string username, INavigation navigation)
        {
            Drive = drive;
            Username = username;
            _navigation = navigation;
            _dialogService = new DialogService();
        }

        async Task DeleteDrive()
        {
            bool answer = await _dialogService.ShowMessage("Delete Drive", "Are you sure you want to delete this drive?", "Yes", "No");
            if (answer)
            {
                DeleteDriveResponse deleteDriveResponse;
                try
                {
                    deleteDriveResponse = await App.Database.DeleteDrive(new DeleteDriveRequest
                    {
                        DriveId = Drive.Id
                    });
                }
                catch (Exception e)
                {
                    await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
                    return;
                }

                if (!deleteDriveResponse.Success)
                {
                    await _dialogService.ShowMessage("Error", "Failed to delete drive", "OK");
                    return;
                }

                else
                {
                    GetPersonResponse getPersonResponse;
                    try
                    {
                        getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                        {
                            Username = Username
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
                            Username = Username
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

                    var rootPage = _navigation.NavigationStack[0];
                    _navigation.InsertPageBefore(mainPage, rootPage);
                    await _navigation.PopToRootAsync();
                }
            }
        }
    }
}