using System;
using System.Collections.Generic;

namespace Driver.API.Dbo
{
    public class DriveDbo
    {
        public int Id { get; set; }
        public string Dest { get; set; }
        public DateTime Date { get; set; }
        public List<DriveParticipantDbo> Participants { get; set; }
        public DriveParticipantDbo Driver { get; set; }
    }
}