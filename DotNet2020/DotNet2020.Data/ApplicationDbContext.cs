using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Core.Auth;
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

        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Seekday> Seekdays { get; set; }
        public DbSet<Illness> Illnesses { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}
