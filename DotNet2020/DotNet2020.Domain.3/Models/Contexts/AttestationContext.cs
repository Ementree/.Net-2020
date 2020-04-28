using System;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Models.Contexts
{
    [Obsolete("Используйте ApplicationDbContext",true)]
    public class AttestationContext:DbContext
    {
        public AttestationContext(DbContextOptions<AttestationContext> options):base(options) { }

        public DbSet<AttestationModel> Attestations { get; set; }
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<AttestationAnswerModel> AttestationAnswer { get; set; }
        public DbSet<GradesModel> Grades { get; set; }
        public DbSet<CompetencesModel> Competences { get; set; }
        public DbSet<GradeCompetencesModel> GradeCompetences { get; set; }
        public DbSet<SpecificWorkerModel> Workers { get; set; }
        public DbSet<SpecificWorkerCompetencesModel> SpecificWorkerCompetences { get; set; }
        
        [Obsolete("Перенесите в метод OnModelCreating3, вызовите этот метод в OnModelCreating в ApplicationDbContext",true)]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
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