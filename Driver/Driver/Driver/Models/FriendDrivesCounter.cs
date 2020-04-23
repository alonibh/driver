namespace Driver.Models
{
    public class FriendDrivesCounter
    {
        public Friend Friend { get; set; }
        public int Counter { get; set; }
        public string CounterStr => GetCounterStr();

        private string GetCounterStr()
        {
            if (Counter == 0)
            {
                return "You're even";
            }

            var counterStr = Counter > 0 ? $"You get {Counter} drives back" : $"You owe {Counter * -1} drives";
            return counterStr;
        }
    }
}
