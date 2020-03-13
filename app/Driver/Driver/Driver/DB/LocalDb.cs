using Driver.DB.DBO;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
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
            _database.CreateTableAsync<DriveDbo>().Wait();
        }

        public Task<UserDbo> GetUserProfile(string username, string password)
        {
            return _database.Table<UserDbo>().Where(o => o.Username == username && o.Password == password).FirstOrDefaultAsync();
        }

        public bool IsUsernameTaken(string username)
        {
            var user = _database.Table<UserDbo>().Where(o => o.Username == username).FirstOrDefaultAsync().Result;
            if (user == null)
                return false;
            return true;
        }

        public int SignUpUser(string username, string password, string firstName, string lastName, string address, Uri image)
        {
            var user = new UserDbo
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Image = image,
                DrivesIds = string.Empty,
                //FriendsIds = string.Empty TODO - When supporting specific friends for each user
            };
            int rowsAdded = _database.InsertAsync(user).Result;
            if (rowsAdded == 0)
                return -1;
            return user.Id;
        }

        public List<DriveDbo> GetDrives(List<int> ids)
        {
            List<DriveDbo> drives = new List<DriveDbo>();
            foreach (var id in ids)
            {
                var drive = _database.Table<DriveDbo>().Where(o => o.Id == id).FirstOrDefaultAsync().Result;
                if (drive != null)
                    drives.Add(drive);
            }
            return drives;
        }

        public Task DeleteDrive(int driveId) =>
            _database.Table<DriveDbo>().DeleteAsync(o => o.Id == driveId);

        public List<FriendDbo> GetFriends(List<int> ids)
        {
            List<FriendDbo> friends = new List<FriendDbo>();
            foreach (var id in ids)
            {
                FriendDbo friend = _database.Table<UserDbo>().Where(o => o.Id == id).FirstOrDefaultAsync().Result;
                if (friend != null)
                    friends.Add(friend);
            }
            return friends;
        }

        public string GetUserFriends(int userId)
        {
            // _database.Table<UserDbo>().Where(o => o.Id == userId).FirstOrDefaultAsync().Result.FriendsIds; TODO - This logic represent a list of friends (user ids) for each user,
            // for now, we will return a list of all of the users ids, meaning that everyone is a friend of everyone
            var allOtherUsers = _database.Table<UserDbo>().Where(o => o.Id != userId).ToListAsync().Result;
            var allOtherUsersStr = JsonConvert.SerializeObject(allOtherUsers);
            return allOtherUsersStr;
        }

        private async Task AddDriveIdToUser(int userId, int driveId)
        {
            var user = await _database.Table<UserDbo>().Where(o => o.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return; // TODO handle correctly
            var drivesIds = JsonConvert.DeserializeObject<List<int>>(user.DrivesIds);
            if (drivesIds == null)
                drivesIds = new List<int>();
            drivesIds.Add(driveId);
            user.DrivesIds = JsonConvert.SerializeObject(drivesIds);
            await _database.UpdateAsync(user);
        }

        public async Task AddDrive(string destination, DateTime date, string participantsStr, string driverStr)
        {
            // TODO full rollback if faild somewhere
            var drive = new DriveDbo
            {
                Date = date,
                Destination = destination,
                Driver = driverStr,
                Participants = participantsStr
            };
            await _database.InsertAsync(drive);

            var participants = JsonConvert.DeserializeObject<List<DriveParticipantDbo>>(participantsStr);
            foreach (var participant in participants)
            {
                await AddDriveIdToUser(participant.Id, drive.Id);
            }

            var driver = JsonConvert.DeserializeObject<DriveParticipantDbo>(driverStr);
            await AddDriveIdToUser(driver.Id, drive.Id);
        }
    }
}