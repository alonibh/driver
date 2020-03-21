using Driver.API.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class Drive
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<DriveParticipant> Participants { get; set; }
        public DriveParticipant Driver { get; set; }
        public string Description => $"{Destination}, {Date:dd/MM/yyyy}";

        public static implicit operator Drive(DriveDbo dbo)
        {
            return new Drive
            {
                Id = dbo._id,
                Date = dbo.Date,
                Destination = dbo.Dest,
                Driver = dbo.Driver,
                Participants = dbo.Participants.Select(o => (DriveParticipant)o).ToList()
            };
        }
    }
}