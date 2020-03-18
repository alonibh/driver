using System;

namespace Driver.Dbo
{
    public class DriveDbo
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public string Participants { get; set; }
        public string Driver { get; set; }
    }
}