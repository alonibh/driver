using System;

namespace Driver.DB.DBO
{
    public class DriveDbo
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public string Participants { get; set; }
        public DriveParticipantDbo Driver { get; set; }
    }
}