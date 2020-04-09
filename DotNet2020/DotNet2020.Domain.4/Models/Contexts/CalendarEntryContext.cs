using DotNet2020.Data;
using DotNet2020.Domain._4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class CalendarEntryContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        public CalendarEntryContext(DbContextOptions<CalendarEntryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
