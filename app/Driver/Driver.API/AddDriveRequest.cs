using System;

namespace Driver.API
{
    public class AddDriveRequest
    {
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public string Participants { get; set; }
        public string Driver { get; set; }
    }
}