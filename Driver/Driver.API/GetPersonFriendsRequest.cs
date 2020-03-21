using Driver.API.Dbo;
using System.Collections.Generic;

namespace Driver.API
{
    public class GetPersonFriendsRequest
    {
        public string Username { get; set; }
    }

    public class GetPersonFriendsResponse
    {
        public List<FriendDbo> Friends { get; set; }
    }
}