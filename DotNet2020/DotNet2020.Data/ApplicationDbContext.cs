using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNet2020.Domain._4.Models;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace DotNet2020.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppIdentityUser, AppIdentityRole, string>
    {
        public virtual DbSet<FunctioningCapacityProject> FunctioningCapacityProjects { get; set; }
        public virtual DbSet<FunctioningCapacityResource> FunctioningCapacityResources { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<ResourceCapacity> ResourceCapacities { get; set; }
        public virtual DbSet<ResourceGroupType> ResourceGroupsTypes { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Recommendation> Recommendations { get; set; }
        public virtual DbSet<AbstractCalendarEntry> AbstractCalendarEntries { get; set; }
        public virtual DbSet<CalendarEntry> CalendarEntries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
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
    }
}
