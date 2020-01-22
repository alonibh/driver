using SQLite;
using System;

namespace Driver.DB.DBO
{
    public class UserDbo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }
        public string DrivesIds { get; set; }
        //public string FriendsIds { get; set; } TODO - When supporting specific friends for each user
    }
}