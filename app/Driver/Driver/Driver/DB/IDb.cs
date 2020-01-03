using Driver.DB.DBO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Driver.DB
{
    public interface IDb
    {
        bool IsUsernameTaken(string username);
        Task<int> SignUpUser(string username, string password, string firstName, string lastName, string address, Uri image);
        string GetUserFriends(int userId);
        Task<UserDbo> GetUserProfile(string username, string password);
        List<DriveDbo> GetDrives(List<int> ids);
        Task DeleteDrive(int driveId);
        List<FriendDbo> GetFriends(List<int> ids);
    }
}