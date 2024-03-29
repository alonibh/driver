﻿using System.Collections.Generic;

namespace Driver.API.Dbo
{
    public class PersonDbo
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public List<FriendDbo> Friends { get; set; }
    }
}