using System;

namespace Driver.DB.DBO
{
    public class FriendDbo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }

        public static implicit operator FriendDbo(UserDbo user)
        {
            return new FriendDbo
            {
                Id = user.Id,
                Address = user.Address,
                FirstName = user.FirstName,
                Image = user.Image,
                LastName = user.LastName
            };
        }
    }
}
