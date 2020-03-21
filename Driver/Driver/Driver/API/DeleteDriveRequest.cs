namespace Driver.API
{
    public class DeleteDriveRequest
    {
        public string DriveId { get; set; }
    }

    public class DeleteDriveResponse
    {
        public bool Success { get; set; }
    }
}