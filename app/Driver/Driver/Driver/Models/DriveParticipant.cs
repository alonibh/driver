﻿using Driver.DB.DBO;

namespace Driver.Models
{
    public class DriveParticipant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;

        public static implicit operator DriveParticipant(DriveParticipantDbo dbo)
        {
            return new DriveParticipant
            {
                Id = dbo.Id,
                FirstName = dbo.FirstName,
                LastName = dbo.LastName,
            };
        }
    }
}