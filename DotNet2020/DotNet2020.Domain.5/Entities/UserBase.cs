using DotNet2020.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet2020.Domain._5.Entities
{
    public abstract class UserBase
    {
        [Required]
        public AppIdentityUser User { get; protected set; }

        public UserBase(AppIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("User was null!");
            User = user;
        }

        protected UserBase() : base() { }
    }
}
