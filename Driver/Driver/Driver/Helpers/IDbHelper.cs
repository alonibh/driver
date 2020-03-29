using Driver.API;
using System.Threading.Tasks;

namespace Driver.Helpers
{
    public interface IDbHelper
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
        Task<DeleteFriendResponse> DeleteFriend(DeleteFriendRequest request);
        void SetToken(string token);
    }
}