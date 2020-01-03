﻿using Driver.DB.DBO;
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

        public Task<int> SignUpUser(string username, string password, string firstName, string lastName, string address, Uri image)
        {
            return _database.InsertAsync(new UserDbo
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Image = image,
                DrivesIds = string.Empty,
                FriendsIds = string.Empty
            });
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

        public string GetUserFriends(int userId)=>
            _database.Table<UserDbo>().Where(o => o.Id == userId).FirstOrDefaultAsync().Result.FriendsIds;
    }
}