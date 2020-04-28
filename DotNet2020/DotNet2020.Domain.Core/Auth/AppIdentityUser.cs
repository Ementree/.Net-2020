﻿using System;
using DotNet2020.Domain.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace DotNet2020.Data
{
    public class AppIdentityUser : IdentityUser
    {
        [Obsolete("Use Employee.FirstName instead", true)]
        public string FirstName { get; protected set; }
        
        [Obsolete("Use Employee.LastName instead", true)]
        public string LastName { get;protected set; }
        
        [Obsolete("Use Employee.Position instead", true)]
        public string Position { get;protected set; }

        
        public Employee Employee { get; set; }
        public string Test { get; protected set; }

        public AppIdentityUser(string userName,string email,string firstName,string lastName,string position, string test)
            :base(userName)
        {
            Email = email;
            //to do validate email!!!
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Test = test;
        }

        // For EF Core only
        protected AppIdentityUser()
            : base()
        {
           
        }
    }
}
