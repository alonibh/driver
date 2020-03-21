using Driver.API.Dbo;
using System.Collections.Generic;

namespace Driver.API
{
    public class GetPersonDrivesRequest
    {
        public string Username { get; set; }
    }

    public class GetPersonDrivesResponse
    {
        public List<DriveDbo> Drives { get; set; }
    }
}