using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DotNet2020.Domain._5.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IStorage _storage;
        private readonly ITimeTrackingService _timeTrackingService;
        private readonly IChartService _chartService;

        public ReportController(ILogger<ReportController> logger, DbContext db)
        {
            _logger = logger;
            _storage = new Storage(db);
            _timeTrackingService = new YouTrackService();
            _chartService = new ChartService();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var ytService = new YouTrackService();
            var model = new CreateReportModel()
            {
                CreateProject = ytService.GetAllProjects()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int reportId)
        {
            var reportResult = _storage.GetReport(reportId);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            var model = new EditReportModel()
            {
                ProjectName = reportResult.Result.Name,
                //UserFilter = ytService.GetAllProjects()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditReportModel model)
        {
            var reportResult = _storage.GetReport(model.ReportId);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            var issues = _timeTrackingService.GetIssues(reportResult.Result.ProjectName);
            if (model.IsWorkItems)
            {
                foreach (var issue in issues)
                    issue.SetTimeByWorkItems();
            }
            var newReport = new Report(reportResult.Result.Name, reportResult.Result.ProjectName, "", issues);

            reportResult = _storage.EditReport(reportResult.Result.ReportId, newReport);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            var charts = _chartService.GetAllCharts();
            foreach (var chart in charts.Values)
                chart.SetData(reportResult.Result.Issues, 5);

            return View(new ChartModel(reportResult.Result.ReportId, charts.Values.ToList()));
        }

        [HttpPost]
        public IActionResult Show(CreateReportModel model)
        {
            // Get issues
            var issues = _timeTrackingService.GetIssues(model.ProjectName, model.IssueFilter)
                .ToList();

            // Save report
            var report = new Report(model.ReportName, model.ProjectName, model.IssueFilter, issues);
            _storage.SaveReport(report);

            // Get saved report with id
            var reportResult = _storage.GetReport(report.Name);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            // Filter issues
            var issuesToShow = issues
                .Where(i => i.SpentTime.HasValue && i.EstimatedTime.HasValue)
                .ToList();

            // Get and fill charts
            var charts = _chartService.GetAllCharts();
            foreach (var chart in charts.Values)
                chart.SetData(issuesToShow, 5);

            return View(new ChartModel(reportResult.Result.ReportId, charts.Values.ToList()));
        }

        [HttpGet]
        public IActionResult Show(int reportId)
        {
            // Get report
            var reportResult = _storage.GetReport(reportId);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            // Filter issues
            var issuesToShow = reportResult.Result.Issues
                .Where(i => i.SpentTime.HasValue && i.EstimatedTime.HasValue)
                .ToList();

            // Get and fill charts
            var charts = _chartService.GetAllCharts();
            foreach (var chart in charts.Values)
                chart.SetData(issuesToShow, 5);

            return View(new ChartModel(reportResult.Result.ReportId, charts.Values.ToList()));
        }
    }
}