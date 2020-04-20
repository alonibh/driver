namespace Driver.Models
{
    public enum FriendRequestStatus
    {
        Accepted, // Friends
        Pending, // This user sent a request and waiting for him to reply
        WaitingForApproval // This user has received a friend request, and needs to reply it
    }
}