using System;
using System.Collections.Generic;
using System.Text;
using DotNet2020.Domain.Core.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
