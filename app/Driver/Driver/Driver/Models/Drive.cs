using Driver.DB.DBO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class Drive
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<DriveParticipant> Participants { get; set; }
        public DriveParticipant Driver { get; set; }
        public string Description => Destination + ", " + Date.ToString("dd/MM/yyyy");
        public static implicit operator Drive(DriveDbo dbo)
        {
            return new Drive
            {
                Id = dbo.Id,
                Date = dbo.Date,
                Destination = dbo.Destination,
                Driver = JsonConvert.DeserializeObject<DriveParticipantDbo>(dbo.Driver),
                Participants = JsonConvert.DeserializeObject<List<DriveParticipantDbo>>(dbo.Participants).Select(o => (DriveParticipant)o).ToList()
            };
        }
    }
}