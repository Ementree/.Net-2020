using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Services
{
    public class Storage : IStorage
    {
        private DbSet<Report> _reports { get; set; }
        private DbContext _db { get; set; }

        public Storage(DbContext db)
        {
            _reports = db.Set<Report>();
            _db = db;
        }

        RequestResult IStorage.SaveReport(Report report)
        {
            return RequestResult.SaveExecute(() =>
            {
                _reports.Add(report);
                _db.SaveChanges();
            });
        }

        RequestResult<Report> IStorage.GetReport(int reportId)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = _reports.Find(reportId);
                _db.Entry(report)
                    .Collection(r => r.Issues).Load();
                report.Issues
                    .ForEach(i => _db.Entry(i).Collection(issue => issue.WorkItems).Load());
                return report;
            });
        }

        RequestResult<Report> IStorage.GetReport(string reportName)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = _reports.FirstOrDefault(r => r.Name == reportName);
                _db.Entry(report)
                    .Collection(r => r.Issues).Load();
                report.Issues
                    .ForEach(i => _db.Entry(i).Collection(issue => issue.WorkItems).Load());
                return report;
            });
        }

        RequestResult IStorage.RemoveReport(int reportId)
        {
            return RequestResult.SaveExecute(() =>
            {
                var report = _reports.Find(reportId);
                if (report == null)
                    return;
                _reports.Remove(report);
                _db.SaveChanges();
            });
        }

        RequestResult IStorage.RemoveReport(string reportName)
        {
            return RequestResult.SaveExecute(() =>
            {
                var report = _reports.FirstOrDefault(r => r.Name == reportName);
                if (report == null)
                    return;
                _reports.Remove(report);
                _db.SaveChanges();
            });
        }

        RequestResult<Report> IStorage.EditReport(int oldReportId, Report newReport)
        {
            return RequestResult<Report>.SaveExecute(() =>
            {
                var report = _reports.Find(oldReportId);
                if (report == null)
                    throw new KeyNotFoundException($"Key {oldReportId} is not found!");
                newReport.ReportId = report.ReportId;
                report = newReport;
                _db.SaveChanges();
            });
        }

        RequestResult<IQueryable<Report>> IStorage.GetAllReports()
        {
            return RequestResult<IQueryable<Report>>.SaveExecute(() =>
            {
                return _reports;
            });
        }
    }
}
