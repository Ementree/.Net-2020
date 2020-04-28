using DotNet2020.Domain._6.Models;
using DotNet2020.Domain.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        
        public virtual DbSet<CalendarEntry> CalendarEntries { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
