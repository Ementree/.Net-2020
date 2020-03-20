using System;
using DotNet2020.Domain._4.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain.Models.Contexts
{
    public class CalendarEntryContext : DbContext
    {
        public CalendarEntryContext(DbContextOptions<CalendarEntryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Illness> Illnesses { get; set; }
        public DbSet<Sickday> Sickdays { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}
