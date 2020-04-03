using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;

namespace Kendo.Mvc.Examples.Models
{
    public partial class SampleEntitiesDataContext : DbContext
    {
        public SampleEntitiesDataContext()
            : base(new DbContextOptions<SampleEntitiesDataContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dataDirectory = Path.Combine("/Data", "App_Data");

            options.UseSqlite(@"Data Source=" + dataDirectory + System.IO.Path.DirectorySeparatorChar + @"sample.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeetingAttendee>(entity =>
            {
                entity.ToTable("MeetingAttendees");

                entity.HasKey(e => new { e.MeetingID, e.AttendeeID });

                entity.Property(e => e.MeetingID).HasColumnType("INT");

                entity.Property(e => e.AttendeeID).HasColumnType("INT");

                entity.HasOne(d => d.Meeting).WithMany(p => p.MeetingAttendees).HasForeignKey(d => d.MeetingID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Tasks");

                entity.HasKey(e => e.TaskID);

                entity.Property(e => e.End)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.IsAllDay)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.OwnerID)
                    .HasColumnType("INT")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.RecurrenceID)
                    .HasColumnType("INT")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Start)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.Recurrence).WithMany(p => p.InverseRecurrence).HasForeignKey(d => d.RecurrenceID);
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.ToTable("Meetings");

                entity.HasKey(e => e.MeetingID);

                entity.Property(e => e.End)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.IsAllDay)
                    .IsRequired()
                    .HasColumnType("BOOLEAN");

                entity.Property(e => e.RecurrenceID)
                    .HasColumnType("INT")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.RoomID)
                    .HasColumnType("INT")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Start)
                    .IsRequired()
                    .HasColumnType("DATETIME");

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.Recurrence).WithMany(p => p.InverseRecurrence).HasForeignKey(d => d.RecurrenceID);
            });
        }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<MeetingAttendee> MeetingAttendees { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
    }
}