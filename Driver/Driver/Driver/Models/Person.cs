using Driver.API.Dbo;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class Person
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<Drive> Drives { get; set; }
        public List<Friend> Friends { get; set; }
        public List<DrivesCounter> DrivesCounter => GetDrivesCounter();
        public string FullName => FirstName + " " + LastName;

        private List<DrivesCounter> GetDrivesCounter()
        {
            if (Drives == null)
            {
                return new List<DrivesCounter>();
            }

            var drivesCounter = new List<DrivesCounter>();
            foreach (var drive in Drives)
            {
                if (drive.Driver.Username == Username) // If you are the driver
                {
                    foreach (var participant in drive.Participants.Where(o => o.Username != Username))
                    {
                        if (drivesCounter.Any(o => o.Username == participant.Username))
                        {
                            drivesCounter.Single(o => o.Username == participant.Username).Counter++;
                        }
                        else
                        {
                            drivesCounter.Add(new DrivesCounter
                            {
                                Counter = 1,
                                FullName = participant.FullName,
                                Username = participant.Username
                            });
                        }
                    }
                }
                else // If you are one of the participants
                {
                    if (drivesCounter.Any(o => o.Username == drive.Driver.Username))
                    {
                        drivesCounter.Single(o => o.Username == drive.Driver.Username).Counter--;
                    }
                    else
                    {
                        drivesCounter.Add(new DrivesCounter
                        {
                            Counter = -1,
                            FullName = drive.Driver.FirstName + " " + drive.Driver.LastName,
                            Username = drive.Driver.Username
                        });
                    }
                }
            }
            return drivesCounter;
        }

        public static implicit operator Person(PersonDbo dbo)
        {
            return new Person
            {
                Username = dbo.Username,
                FirstName = dbo.FirstName,
                LastName = dbo.LastName,
                Address = dbo.LastName,
                Email = dbo.Email
            };
        }
    }
}