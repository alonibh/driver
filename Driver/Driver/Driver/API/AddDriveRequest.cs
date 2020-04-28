using System;
using System.Collections.Generic;

namespace Driver.API
{
    public class AddDriveRequest
    {
        public string Driver { get; set; }
        public List<string> Participants { get; set; }
        public string Dest { get; set; }
        public DateTime Date { get; set; }
    }

    public class AddDriveResponse
    {
        public bool Success { get; set; }
    }
}