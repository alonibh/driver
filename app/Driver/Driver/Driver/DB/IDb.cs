using Driver.DB.DBO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Driver.DB
{
    public interface IDb
    {
        Task<UserDbo> Login(string username, string password);
        int SignUp(string username, string password, string firstName, string lastName, string address, Uri image);
        Task AddDrive(string destination, DateTime date, string participantsStr, string driverStr);
        List<DriveDbo> GetDrives(List<int> ids);
        Task DeleteDrive(int driveId);
        //UpdateDrive(int driveId, ...)
        string GetUserFriends(int userId);
        List<FriendDbo> GetFriends(List<int> ids);
    }
}