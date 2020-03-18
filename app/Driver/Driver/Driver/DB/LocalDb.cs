using Driver.API;
using Driver.Dbo;
using Driver.Models;
using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Driver.DB
{
    public class LocalDb : IDb
    {
        readonly SQLiteAsyncConnection _database;

        public LocalDb(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<UserDbo>().Wait();
            _database.CreateTableAsync<PersonWithDrivesDbo>().Wait();
            _database.CreateTableAsync<DriveDbo>().Wait();
        }

        public async Task<bool> Login(LoginRequest request)
        {
            var user = await _database.Table<UserDbo>().Where(o => o.Username == request.Username && o.Password == request.Password).FirstOrDefaultAsync();
            return user == null ? false : true;
        }

        public async Task<bool> SignUp(SignupRequest request)
        {
            var user = await _database.Table<UserDbo>().Where(o => o.Username == request.Username).FirstOrDefaultAsync();
            if (user != null)
                return false;

            var person = new PersonWithDrivesDbo
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Email = request.Email,
                FriendsUsernames = string.Empty,
                DrivesIds = string.Empty
            };

            int rowsAdded = await _database.InsertAsync(person);
            if (rowsAdded == 0)
                return false;

            user = new UserDbo
            {
                Username = request.Username,
                Password = request.Password
            };

            rowsAdded = await _database.InsertAsync(user);
            if (rowsAdded == 0)
                return false;

            return true;
        }

        public async Task<bool> AddDrive(AddDriveRequest request)
        {
            var drive = new DriveDbo
            {
                Date = request.Date,
                Destination = request.Destination,
                Driver = request.Driver,
                Participants = request.Participants
            };
            await _database.InsertAsync(drive);

            var participants = JsonConvert.DeserializeObject<List<Person>>(request.Participants);
            foreach (var participant in participants)
            {
                await AddDriveIdToUser(participant.Username, drive.Id);
            }

            var driver = JsonConvert.DeserializeObject<Person>(request.Driver);
            await AddDriveIdToUser(driver.Username, drive.Id);
            return true;
        }

        public async Task<DriveDbo> GetDrive(GetDriveRequest request)
        {
            var drive = await _database.Table<DriveDbo>().Where(o => o.Id == request.DriveId).FirstOrDefaultAsync();
            return drive;
        }

        public async Task<bool> DeleteDrive(DeleteDriveRequest request)
        {
            int deletedRows = await _database.Table<DriveDbo>().DeleteAsync(o => o.Id == request.DriveId);
            if (deletedRows == 0)
                return false;
            return true;
        }

        public async Task<PersonDbo> GetPerson(GetPersonRequest request)
        {
            var persons = await _database.Table<PersonWithDrivesDbo>().ToListAsync();
            var person = persons.Where(o => o.Username == request.Username).FirstOrDefault();
            persons.Remove(person);
            List<string> friendsUsernames = new List<string>();
            foreach (var otherPerson in persons)
            {
                friendsUsernames.Add(otherPerson.Username);
            }
            person.FriendsUsernames = JsonConvert.SerializeObject(friendsUsernames);
            return person;
        }

        public async Task<List<DriveDbo>> GetPersonDrives(GetPersonDrivesRequest request)
        {
            var person = await _database.Table<PersonWithDrivesDbo>().Where(o => o.Username == request.Username).FirstOrDefaultAsync();
            var drivesIds = JsonConvert.DeserializeObject<List<int>>(person.DrivesIds);
            List<DriveDbo> drives = new List<DriveDbo>();
            if (drivesIds == null)
                return drives;
            foreach (int driveId in drivesIds)
            {
                var drive = await GetDrive(new GetDriveRequest { DriveId = driveId });
                if (drive != null)
                    drives.Add(drive);
            }

            string drivesIdsSerialized = JsonConvert.SerializeObject(drives);
            person.DrivesIds = drivesIdsSerialized;
            await _database.UpdateAsync(person);

            return drives;
        }

        private async Task AddDriveIdToUser(string username, int driveId)
        {
            var person = await _database.Table<PersonWithDrivesDbo>().Where(o => o.Username == username).FirstOrDefaultAsync();
            if (person == null)
                return; // TODO handle correctly
            var drivesIds = JsonConvert.DeserializeObject<List<int>>(person.DrivesIds);
            if (drivesIds == null)
                drivesIds = new List<int>();
            drivesIds.Add(driveId);
            person.DrivesIds = JsonConvert.SerializeObject(drivesIds);
            await _database.UpdateAsync(person);
        }

        private class UserDbo
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class PersonWithDrivesDbo
        {
            [PrimaryKey]
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string FriendsUsernames { get; set; }
            public string DrivesIds { get; set; }
            public static implicit operator PersonDbo(PersonWithDrivesDbo dbo)
            {
                return new PersonDbo
                {
                    Username = dbo.Username,
                    FirstName = dbo.FirstName,
                    LastName = dbo.LastName,
                    Address = dbo.LastName,
                    Email = dbo.Email,
                    FriendsUsernames = dbo.FriendsUsernames
                };
            }
        }
    }
}