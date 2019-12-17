using Driver.DB.DBO;
using System;
using System.Threading.Tasks;

namespace Driver.DB
{
    public interface IDb
    {
        bool IsUsernameTaken(string username);
        Task<int> SignupUser(string username, string password, string firstName, string lastName, string address, Uri image);
        Task<UserDbo> GetUserProfile(string username, string password);
    }
}