using System;
using System.Collections.Generic;

namespace Driver.API
{
    public class AddDriveRequest
    {
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<string> Participants { get; set; }
        public string Driver { get; set; }
    }

    public class AddDriveResponse
    {
        public bool Success { get; set; }
    }
}