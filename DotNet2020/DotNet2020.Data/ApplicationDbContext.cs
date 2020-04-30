using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain.Core.Models;

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
        public virtual DbSet<AttestationModel> Attestations { get; set; }
        public virtual DbSet<AnswerModel> Answers { get; set; }
        public virtual DbSet<AttestationAnswerModel> AttestationAnswer { get; set; }
        public virtual DbSet<GradesModel> Grades { get; set; }
        public virtual DbSet<CompetencesModel> Competences { get; set; }
        public virtual DbSet<GradeCompetencesModel> GradeCompetences { get; set; }
        public virtual DbSet<SpecificWorkerModel> Employees { get; set; }
        public virtual DbSet<SpecificWorkerCompetencesModel> SpecificWorkerCompetences { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<CalendarEntry> CalendarEntries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            OnModelCreating3(builder);
        }

        private void OnModelCreating3(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecificWorkerCompetencesModel>()
                .HasKey(e => new { e.WorkerId, e.CompetenceId });
            modelBuilder.Entity<GradeCompetencesModel>()
                .HasKey(e => new { e.GradeId, e.CompetenceId });
            modelBuilder.Entity<AttestationAnswerModel>()
                .HasKey(e => new { e.AttestationId, e.AnswerId });

            modelBuilder.Entity<SpecificWorkerCompetencesModel>()
                .HasOne<SpecificWorkerModel>(e => e.Worker)
                .WithMany(p => p.SpecificWorkerCompetencesModels)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SpecificWorkerCompetencesModel>()
                .HasOne<CompetencesModel>(e => e.Competence)
                .WithMany(p => p.SpecificWorkerCompetencesModels)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GradeCompetencesModel>()
                .HasOne<GradesModel>(e => e.Grade)
                .WithMany(p => p.GradesCompetences)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<GradeCompetencesModel>()
                .HasOne<CompetencesModel>(e => e.Competence)
                .WithMany(p => p.GradesCompetences)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AttestationAnswerModel>()
                .HasOne<AttestationModel>(e => e.Attestation)
                .WithMany(p => p.AttestationAnswer)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AttestationAnswerModel>()
                .HasOne<AnswerModel>(e => e.Answer)
                .WithMany(p => p.AttestationAnswer)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
