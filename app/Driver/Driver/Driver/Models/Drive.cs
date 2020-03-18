﻿using Driver.API.Dbo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Driver.Models
{
    public class Drive
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public List<Person> Participants { get; set; }
        public Person Driver { get; set; }
        public string Description => Destination + ", " + Date.ToString("dd/MM/yyyy");

        public static implicit operator Drive(DriveDbo dbo)
        {
            return new Drive
            {
                Id = dbo.Id,
                Date = dbo.Date,
                Destination = dbo.Destination,
                Driver = JsonConvert.DeserializeObject<PersonDbo>(dbo.Driver),
                Participants = JsonConvert.DeserializeObject<List<PersonDbo>>(dbo.Participants).Select(o => (Person)o).ToList()
            };
        }
    }
}