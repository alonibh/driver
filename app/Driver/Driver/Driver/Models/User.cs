using Driver.DB.DBO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Driver.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Uri Image { get; set; }
        public List<Drive> Drives { get; set; }
        public static implicit operator User(UserDbo dbo)
        {
            return new User
            {
                Address = dbo.Address,
                Drives = JsonConvert.DeserializeObject<List<Drive>>(dbo.Drives),
                FirstName = dbo.FirstName,
                ID = dbo.ID,
                Image = dbo.Image,
                LastName = dbo.LastName
            };
        }
    }
}