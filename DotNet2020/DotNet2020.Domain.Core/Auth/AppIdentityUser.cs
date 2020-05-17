using System;
using DotNet2020.Domain.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace DotNet2020.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public Employee Employee { get; set; }
        public AppIdentityUser(string userName, string email)
            : base(userName)
        {
            Email = email;
            //to do validate email!!!
        }

        // For EF Core only
        public AppIdentityUser()
            : base()
        {
           
        }
    }
}
