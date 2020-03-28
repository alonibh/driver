namespace Driver.API
{
    public class AddFriendRequest
    {
        public string Username { get; set; }
    }

    public class AddFriendResponse
    {
        public bool Success { get; set; }
    }
}