namespace Driver.API
{
    public class DeleteFriendRequest
    {
        public string Username { get; set; }
    }

    public class DeleteFriendResponse
    {
        public bool Success { get; set; }
    }
}