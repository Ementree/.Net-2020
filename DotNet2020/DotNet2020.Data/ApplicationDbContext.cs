using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain._6.Models;

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
        public virtual DbSet<ResourceGroupType> ResourceTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
