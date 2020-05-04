using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Services
{
    public class Storage : DbContext, IStorage
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Issue> Issues { get; set; }

        public Storage(DbContextOptions<Storage> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .HasMany(i => i.WorkItems)
                .WithOne()
                .HasForeignKey(w => w.IssueId);
            modelBuilder.Entity<Report>()
                .HasMany(r => r.Issues)
                .WithOne()
                .HasForeignKey(i => i.ReportId);
        }

        RequestResult IStorage.SaveReport(Report report)
        {
            return RequestResult.SaveExecute(() =>
            {
                Reports.Add(report);
                SaveChanges();
            });
        }

        RequestResult<Report> IStorage.GetReport(int reportId)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = Reports.Find(reportId);
                return report;
            });
        }

        RequestResult<Report> IStorage.GetReport(string reportName)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = Reports.FirstOrDefault(r => r.Name == reportName);
                return report;
            });
        }

        RequestResult IStorage.RemoveReport(int reportId)
        {
            return RequestResult.SaveExecute(() =>
            {
                var report = Reports.Find(reportId);
                if (report == null)
                    return;
                Reports.Remove(report);
                SaveChanges();
            });
        }

        RequestResult IStorage.RemoveReport(string reportName)
        {
            return RequestResult.SaveExecute(() =>
            {
                var report = Reports.FirstOrDefault(r => r.Name == reportName);
                if (report == null)
                    return;
                Reports.Remove(report);
                SaveChanges();
            });
        }

        RequestResult<Report> IStorage.EditReport(int oldReportId, Report newReport)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = Reports.Find(oldReportId);
                if (report == null)
                    throw new KeyNotFoundException($"Key {oldReportId} is not found!");
                newReport.ReportId = report.ReportId;
                report = newReport;
                SaveChanges();
            });
        }

        RequestResult<IQueryable<Report>> IStorage.GetAllReports()
        {
            return RequestResult<IQueryable<Report>>.SaveExecute(() =>
            {
                return Reports;
            });
        }
    }
}
