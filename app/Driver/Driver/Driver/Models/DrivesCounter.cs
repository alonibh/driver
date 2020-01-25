namespace Driver.Models
{
    public class DrivesCounter
    {
        public string FullName { get; set; }
        public int ID { get; set; }
        public int Counter { get; set; }
        public string CounterStr => GetCounterStr();
        private string GetCounterStr()
        {
            if (Counter == 0)
                return "You're even";
            var returnStr = Counter > 0 ? $"You get {Counter} drives back" : $"You owe {Counter*-1} drives";
            return returnStr;
        }
    }
}