using Driver.API.Dbo;

namespace Driver.API
{
    public class GetDriveRequest
    {
        public int DriveId { get; set; }
    }

    public class GetDriveResponse
    {
        public DriveDbo Drive { get; set; }
    }
}