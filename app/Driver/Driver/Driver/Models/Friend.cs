using Driver.DB.DBO;
using System;

namespace Driver.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }
        public string FullName => FirstName + " " + LastName;


        public static implicit operator Friend(FriendDbo dbo)
        {
            return new Friend
            {
                Id = dbo.Id,
                Address = dbo.Address,
                FirstName = dbo.FirstName,
                Image = dbo.Image,
                LastName = dbo.LastName
            };
        }
    }
}