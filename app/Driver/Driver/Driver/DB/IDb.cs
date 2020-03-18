using Driver.API;
using Driver.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Driver.DB
{
    public interface IDb
    {
        Task<bool> Login(LoginRequest request);
        Task<bool> SignUp(SignupRequest request);
        Task<bool> AddDrive(AddDriveRequest request);
        Task<DriveDbo> GetDrive(GetDriveRequest request);
        Task<bool> DeleteDrive(DeleteDriveRequest request);
        Task<PersonDbo> GetPerson(GetPersonRequest request);
        Task<List<DriveDbo>> GetPersonDrives(GetPersonDrivesRequest request);
    }
}