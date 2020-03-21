using Driver.API;
using Driver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Driver.MainPages
{
    public partial class DriveInfoPage : ContentPage
    {
        public DriveInfoPage()
        {
            InitializeComponent();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Delete Drive", "Are you sure you want to delete this drive?", "Yes", "No");
            if (answer)
            {
                var driveInfo = (DriveInfo)BindingContext;

                DeleteDriveResponse deleteDriveResponse;
                try
                {
                    deleteDriveResponse = await App.Database.DeleteDrive(new DeleteDriveRequest
                    {
                        DriveId = driveInfo.Drive.Id
                    });
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                    return;
                }

                if (!deleteDriveResponse.Success)
                {
                    await DisplayAlert("Error", "Failed to delete drive", "OK");
                    return;
                }

                else
                {
                    var mainPage = Navigation.NavigationStack[0];

                    GetPersonResponse getPersonResponse;
                    try
                    {
                        getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                        {
                            Username = driveInfo.Username
                        }));
                    }
                    catch (Exception e)
                    {
                        await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                        return;
                    }

                    GetPersonDrivesResponse getPersonDrivesResponse;
                    try
                    {
                        getPersonDrivesResponse = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                        {
                            Username = driveInfo.Username
                        }));
                    }
                    catch (Exception e)
                    {
                        await DisplayAlert("Error", $"The server returned an error: {e.Message}", "OK");
                        return;
                    }


                    Navigation.NavigationStack[0].BindingContext = new Person
                    {
                        Username = getPersonResponse.Person.Username,
                        Address = getPersonResponse.Person.Address,
                        FirstName = getPersonResponse.Person.FirstName,
                        LastName = getPersonResponse.Person.LastName,
                        Email = getPersonResponse.Person.Email,
                        Drives = getPersonDrivesResponse.Drives.Select(o => (Drive)o).ToList(),
                        Friends = new List<Friend>()
                    };

                    await Navigation.PopToRootAsync();
                }
            }
        }
    }
}