using System;
using System.Collections.Generic;

namespace Driver.API.Dbo
{
    public class DriveDbo
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming Styles
        public string _id { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CA1707 // Identifiers should not contain underscores
        public string Dest { get; set; }
        public DateTime Date { get; set; }
        public List<DriveParticipantDbo> Participants { get; set; }
        public DriveParticipantDbo Driver { get; set; }
    }
}