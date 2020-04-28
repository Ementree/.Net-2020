using System;
using DotNet2020.Domain._4.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    [Obsolete("Используйте ApplicationDbContext",true)]
    public class CalendarEntryContext : DbContext
    {
        public CalendarEntryContext(DbContextOptions<CalendarEntryContext> options) : base(options)
        {
        }

        [Obsolete("Перенесите в метод OnModelCreating4, вызовите этот метод в OnModelCreating в ApplicationDbContext",true)]
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

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public virtual DbSet<AbstractCalendarEntry> CalendarEntries { get; set; }
    }
}
