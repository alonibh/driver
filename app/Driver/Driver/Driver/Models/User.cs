using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }
        public List<Drive> Drives { get; set; }
        public List<DrivesCounter> DrivesCounter => GetDrivesCounter();

        private List<DrivesCounter> GetDrivesCounter()
        {
            if (Drives == null)
                return new List<DrivesCounter>();
            var drivesCounter = new List<DrivesCounter>();
            foreach (var drive in Drives)
            {
                if (drive.Driver.ID == ID) // If you are the driver
                {
                    foreach (var participant in drive.Participants)
                    {
                        if (drivesCounter.Any(o => o.ID == participant.ID))
                        {
                            drivesCounter.Single(o => o.ID == participant.ID).Counter++;
                        }
                        else
                        {
                            drivesCounter.Add(new DrivesCounter
                            {
                                Counter = 1,
                                FullName = participant.FirstName + " " + participant.LastName,
                                ID = participant.ID
                            });
                        }
                    }
                }
                else // If you are one of the participants
                {
                    if (drivesCounter.Any(o => o.ID == drive.Driver.ID))
                    {
                        drivesCounter.Single(o => o.ID == drive.Driver.ID).Counter--;
                    }
                    else
                    {
                        drivesCounter.Add(new DrivesCounter
                        {
                            Counter = -1,
                            FullName = drive.Driver.FirstName + " " + drive.Driver.LastName,
                            ID = drive.Driver.ID
                        });
                    }
                }
            }
            return drivesCounter;
        }
    }
}