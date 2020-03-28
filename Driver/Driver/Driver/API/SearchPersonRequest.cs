using Driver.API.Dbo;
using System.Collections.Generic;

namespace Driver.API
{
    public class SearchPersonRequest
    {
        public string Query { get; set; }
    }

    public class SearchPersonResponse
    {
        public List<FriendDbo> Results { get; set; }
    }
}