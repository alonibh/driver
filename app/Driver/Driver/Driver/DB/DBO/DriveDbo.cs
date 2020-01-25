using SQLite;
using System;

namespace Driver.DB.DBO
{
    public class DriveDbo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public string Participants { get; set; }
        public string Driver { get; set; }
    }
}