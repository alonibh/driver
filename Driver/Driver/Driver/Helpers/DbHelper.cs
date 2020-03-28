﻿using Driver.API;
using System;
using System.Threading.Tasks;

namespace Driver.Helpers
{
    public class RemoteDbHelper : IDbHelper
    {
        private readonly DialogService _dialogService;

        public RemoteDbHelper()
        {
            _dialogService = new DialogService();
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            LoginResponse loginResponse = new LoginResponse();
            try
            {
                loginResponse = await App.Database.Login(new LoginRequest
                {
                    Username = request.Username,
                    Password = request.Password
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return loginResponse;
        }

        public async Task<SignupResponse> SignUp(SignupRequest request)
        {
            SignupResponse signupResponse = new SignupResponse();
            try
            {
                signupResponse = await App.Database.SignUp(new SignupRequest
                {
                    Username = request.Username,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Address = request.Address,
                    Email = request.Email
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return signupResponse;
        }

        public async Task<AddDriveResponse> AddDrive(AddDriveRequest request)
        {
            AddDriveResponse addDriveResponse = new AddDriveResponse();
            try
            {
                addDriveResponse = await App.Database.AddDrive(new AddDriveRequest
                {
                    Dest = request.Dest,
                    Date = request.Date,
                    Driver = request.Driver,
                    Participants = request.Participants
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return addDriveResponse;
        }

        public async Task<GetDriveResponse> GetDrive(GetDriveRequest request)
        {
            GetDriveResponse getDriveResponse = new GetDriveResponse();
            try
            {
                getDriveResponse = await App.Database.GetDrive(new GetDriveRequest
                {
                    DriveId = request.DriveId
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return getDriveResponse;
        }

        public async Task<DeleteDriveResponse> DeleteDrive(DeleteDriveRequest request)
        {
            DeleteDriveResponse deleteDriveResponse = new DeleteDriveResponse();
            try
            {
                deleteDriveResponse = await App.Database.DeleteDrive(new DeleteDriveRequest
                {
                    DriveId = request.DriveId
                });
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return deleteDriveResponse;
        }

        public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
        {
            GetPersonResponse getPersonResponse = new GetPersonResponse();
            try
            {
                getPersonResponse = (await App.Database.GetPerson(new GetPersonRequest
                {
                    Username = request.Username
                }));
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return getPersonResponse;
        }

        public async Task<GetPersonDrivesResponse> GetPersonDrives(GetPersonDrivesRequest request)
        {
            GetPersonDrivesResponse getPersonDrivesResponse = new GetPersonDrivesResponse();
            try
            {
                getPersonDrivesResponse = (await App.Database.GetPersonDrives(new GetPersonDrivesRequest
                {
                    Username = request.Username
                }));
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return getPersonDrivesResponse;
        }

        public async Task<GetPersonFriendsResponse> GetPersonFriends(GetPersonFriendsRequest request)
        {
            GetPersonFriendsResponse getPersonFriendsResponse = new GetPersonFriendsResponse();
            try
            {
                getPersonFriendsResponse = (await App.Database.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = request.Username
                }));
            }
            catch (Exception e)
            {
                await _dialogService.ShowMessage("Error", $"The server returned an error: {e.Message}", "OK");
            }
            return getPersonFriendsResponse;
        }

        public void SetToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}