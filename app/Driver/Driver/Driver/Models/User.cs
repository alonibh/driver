using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }
        public List<Drive> Drives { get; set; }
        public List<Friend> Friends { get; set; }
        public List<DrivesCounter> DrivesCounter => GetDrivesCounter();

        private List<DrivesCounter> GetDrivesCounter()
        {
            if (Drives == null)
                return new List<DrivesCounter>();
            var drivesCounter = new List<DrivesCounter>();
            foreach (var drive in Drives)
            {
                if (drive.Driver.Id == Id) // If you are the driver
                {
                    foreach (var participant in drive.Participants)
                    {
                        if (drivesCounter.Any(o => o.ID == participant.Id))
                        {
                            drivesCounter.Single(o => o.ID == participant.Id).Counter++;
                        }
                        else
                        {
                            drivesCounter.Add(new DrivesCounter
                            {
                                Counter = 1,
                                FullName = participant.FirstName + " " + participant.LastName,
                                ID = participant.Id
                            });
                        }
                    }
                }
                else // If you are one of the participants
                {
                    if (drivesCounter.Any(o => o.ID == drive.Driver.Id))
                    {
                        drivesCounter.Single(o => o.ID == drive.Driver.Id).Counter--;
                    }
                    else
                    {
                        drivesCounter.Add(new DrivesCounter
                        {
                            Counter = -1,
                            FullName = drive.Driver.FirstName + " " + drive.Driver.LastName,
                            ID = drive.Driver.Id
                        });
                    }
                }
            }
            return drivesCounter;
        }
    }
}