using Driver.DB.DBO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Driver.Models
{
    public class Drive
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<DriveParticipant> Participants { get; set; }
        public DriveParticipant Driver { get; set; }
        public string Description => Destination + ", " + Date.ToString("dd/MM/yyyy");
        public static implicit operator Drive(DriveDbo dbo)
        {
            return new Drive
            {
                Date = dbo.Date,
                Destination = dbo.Destination,
                Driver = dbo.Driver,
                Name = dbo.Name,
                Participants = JsonConvert.DeserializeObject<List<DriveParticipant>>(dbo.Participants)
            };
        }
    }
}