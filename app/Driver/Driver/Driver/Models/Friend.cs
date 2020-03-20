using Driver.API.Dbo;
using System;

namespace Driver.Models
{
    public class Friend
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public FriendRequestStatus Status { get; set; }
        public string FullName => FirstName + " " + LastName;

        public static implicit operator Friend(FriendDbo dbo)
        {
            Enum.TryParse(dbo.Status, out FriendRequestStatus Status);
            return new Friend
            {
                Username = dbo.Username,
                FirstName = dbo.FirstName,
                LastName = dbo.LastName,
                Address = dbo.Address,
                Status = Status
            };
        }
    }
}