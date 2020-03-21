namespace Driver.Models
{
    public class DriveInfo
    {
        public Drive Drive { get; set; }
        public string Username { get; set; }
        public bool IsUserDriver => Drive.Driver.Username == Username;
    }
}