using System;
using DotNet2020.Domain.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace DotNet2020.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public Employee Employee { get; set; }
        public string Test { get; protected set; }

        public AppIdentityUser(string userName,string email,string firstName,string lastName,string position, string test)
            :base(userName)
        {
            Email = email;
            //to do validate email!!!
            Test = test;
        }

        // For EF Core only
        protected AppIdentityUser()
            : base()
        {
           
        }
    }
}
