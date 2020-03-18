namespace Driver.API
{
    public class DeleteDriveRequest
    {
        public int DriveId { get; set; }
    }

    public class DeleteDriveResponse
    {
        public bool Success { get; set; }
    }
}