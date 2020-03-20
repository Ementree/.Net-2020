using System;
using DotNet2020.Domain._4.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain.Models
{
    public class CalendarEntryContext : DbContext
    {
        public CalendarEntryContext(DbContextOptions<CalendarEntryContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbstractCalendarEntry>()
                .HasDiscriminator<AbsenceType>(nameof(AbstractCalendarEntry.AbsenceType))
                .HasValue<Vacation>
                    (AbsenceType.Vacation)
                .HasValue<SickDay>
                    (AbsenceType.SickDay)
                .HasValue<Illness>
                    (AbsenceType.Illness);
        }


        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Illness> Illnesses { get; set; }
        public DbSet<SickDay> SickDays { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}
