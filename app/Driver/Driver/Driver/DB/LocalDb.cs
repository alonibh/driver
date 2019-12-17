using Driver.DB.DBO;
using SQLite;
using System;
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

        public Task<int> SignupUser(string username, string password, string firstName, string lastName, string address, Uri image)
        {
            return _database.InsertAsync(new UserDbo
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Image = image
            });
        }
    }
}