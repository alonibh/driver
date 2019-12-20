using Driver.DB.DBO;

namespace Driver.Models
{
    public class DriveParticipant
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public static implicit operator DriveParticipant(DriveParticipantDbo dbo)
        {
            return new DriveParticipant
            {
                ID = dbo.ID,
                FirstName = dbo.FirstName,
                LastName = dbo.LastName,
            };
        }
    }
}