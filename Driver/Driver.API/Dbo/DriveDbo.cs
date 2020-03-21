﻿using System;
using System.Collections.Generic;

namespace Driver.API.Dbo
{
    public class DriveDbo
    {
        public int Id { get; set; }
        public string Dest { get; set; }
        public DateTime Date { get; set; }
        public List<string> Participants { get; set; }
        public string Driver { get; set; }
    }
}