using Driver.API;
using System;
using System.Threading.Tasks;

namespace Driver.DB
{
    public interface IDb : IDisposable
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<SignupResponse> SignUp(SignupRequest request);
        Task<AddDriveResponse> AddDrive(AddDriveRequest request);
        Task<GetDriveResponse> GetDrive(GetDriveRequest request);
        Task<DeleteDriveResponse> DeleteDrive(DeleteDriveRequest request);
        Task<GetPersonResponse> GetPerson(GetPersonRequest request);
        Task<GetPersonDrivesResponse> GetPersonDrives(GetPersonDrivesRequest request);
        Task<GetPersonFriendsResponse> GetPersonFriends(GetPersonFriendsRequest request);
        Task<SearchPersonResponse> SearchPerson(SearchPersonRequest request);
        Task<AddFriendResponse> AddFriend(AddFriendRequest request);
        void SetToken(string token);
    }
}