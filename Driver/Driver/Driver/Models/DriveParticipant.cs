using Driver.API.Dbo;

namespace Driver.Models
{
    public class DriveParticipant
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string FullName => FirstName + " " + LastName;

        public static implicit operator DriveParticipant(DriveParticipantDbo dbo)
        {
            return new DriveParticipant
            {
                Username = dbo.Username,
                FirstName = dbo.FirstName,
                LastName = dbo.LastName,
                Address = dbo.Address
            };
        }
    }
}
