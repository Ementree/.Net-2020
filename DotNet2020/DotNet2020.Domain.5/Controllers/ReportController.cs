using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IStorage _storage;
        private readonly ITimeTrackingService _timeTrackingService;
        private readonly IChartService _chartService;

        public ReportController(IConfiguration configuration, ILogger<ReportController> logger, DbContext db)
        {
            _logger = logger;
            _storage = new Storage(db);
            _timeTrackingService = new YouTrackService(configuration);
            _chartService = new ChartService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Available()
        {
            var reportsResult = _storage.GetAllReports();
            if (!reportsResult.IsSuccess)
                Error("Ошибка обращения к БД!", reportsResult.Error.Message);
            var model = new AvailableReportsModel()
            {
                Reports = reportsResult.Result.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var projectNames = _timeTrackingService.GetAllProjects();
            var usersInProjects = new List<string[]>();
            foreach (var projectName in projectNames)
            {
                var userNames = _timeTrackingService.GetAllUsers(projectName);
                userNames.Insert(0, "");
                usersInProjects.Add(userNames.ToArray());
            }
            var model = new CreateReportModel()
            {
                ProjectNames = projectNames,
                UsersInProjects = usersInProjects.ToArray()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateReportModel model)
        {
            if (!ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.ReportName))
                    return Error("Ошибка создания отчета!", "Название отчета должно быть не нулевым!");
                if (model.ReportName?.Length > 100)
                    return Error("Ошибка создания отчета!", "Название отчета должно быть не длиннее 100 символов!");
                if (model.IssueFilter?.Length > 1000)
                    return Error("Ошибка создания отчета!", "Фильтр должен быть не длиннее 1000 символов!");
            }

            RequestResult<bool> isContains = _storage.ContainsReport(model.ReportName);
            if (!isContains.IsSuccess)
                return Error("Ошибка обращения к БД!", isContains.Error.Message);
            if (isContains.Result)
                return Error("Ошибка создания отчета!", "Отчет с таким именем уже существует!");

            var filter = model.IssueFilter ?? "";
            filter = _timeTrackingService.AddDateToFilter(filter, model.Start, model.End);
            if (!String.IsNullOrEmpty(model.UserName))
                filter = _timeTrackingService.AddAssigneeToFilter(filter, model.UserName);

            // Get issues
            var issues = _timeTrackingService.GetIssues(model.ProjectName, filter);

            if (issues == null)
                issues = new List<Issue>();

            // Save report
            var report = new Report(model.ReportName, model.ProjectName, filter, issues);
            _storage.SaveReport(report);

            // Get saved report with id
            var reportResult = _storage.GetReport(report.Name);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            return RedirectToAction("Show", new { id = reportResult.Result.ReportId });
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            Report report = reportResult.Result;

            var projectName = report.ProjectName;

            var users = _timeTrackingService.GetAllUsers(projectName);
            if (users == null)
                return Error("Ошибка обращения к сервису трекинга!", $"Не удалось получить список пользователей проекта \"{projectName}\".");
            users.Insert(0, "");

            var dates = _timeTrackingService.GetDateRangeFromFilter(report.IssueFilter);
            var assignee = _timeTrackingService.GetAssingeeFromFilter(report.IssueFilter);

            var model = new EditReportModel()
            {
                ReportId = id,
                ProjectName = projectName,
                AllUsers = users,
                ReportName = report.Name,
                IssueFilter = report.IssueFilter,
                Start = dates.start,
                End = dates.end,
                SelectedUser = assignee
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ProcessEdit(EditReportModel model)
        {
            if (!ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.ReportName))
                    return Error("Ошибка редактирования отчета!", "Название отчета должно быть не нулевым!");
                if (model.ReportName?.Length > 100)
                    return Error("Ошибка редактирования отчета!", "Название отчета должно быть не длиннее 100 символов!");
                if (model.IssueFilter?.Length > 1000)
                    return Error("Ошибка редактирования отчета!", "Фильтр должен быть не длиннее 1000 символов!");
                return Error("Ошибка редактирования отчета!");
            }

            // Get report
            var reportResult = _storage.GetReport(model.ReportId);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            // Check is name unique
            if (reportResult.Result.Name != model.ReportName)
            {
                var isContains = _storage.ContainsReport(model.ReportName);
                if (!isContains.IsSuccess)
                    return Error("Ошибка обращения к БД!", isContains.Error.Message);
                if (isContains.Result)
                    return Error("Ошибка редактирования отчета!", "Название отчета должно быть уникальным!");
            }

            // Get issues
            var filter = model.IssueFilter ?? "";
            filter = _timeTrackingService.AddDateToFilter(filter, model.Start, model.End);
            filter = _timeTrackingService.AddAssigneeToFilter(filter, model.SelectedUser);

            var issues = _timeTrackingService.GetIssues(reportResult.Result.ProjectName, filter);

            // Change time mode
            if (model.IsWorkItems)
            {
                foreach (var issue in issues)
                    issue.SetTimeByWorkItems();
            }

            // Save new report
            var newReport = new Report(reportResult.Result.Name, reportResult.Result.ProjectName, filter, issues);
            reportResult = _storage.EditReport(reportResult.Result.ReportId, newReport);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            return RedirectToAction("Show", new { id = reportResult.Result.ReportId });
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            // Get report
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

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

        [HttpPost]
        public IActionResult Show(int id, int count)
        {
            if (count <= 2)
                count = 2;
            // Get report
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            // Filter issues
            var issuesToShow = reportResult.Result.Issues
                .Where(i => i.SpentTime.HasValue && i.EstimatedTime.HasValue)
                .ToList();

            // Get and fill charts
            var charts = _chartService.GetAllCharts();
            foreach (var chart in charts.Values)
                chart.SetData(issuesToShow, count);

            return View(new ChartModel(reportResult.Result.ReportId, charts.Values.ToList()));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var result = _storage.RemoveReport(id);
            if (!result.IsSuccess)
                return Error("Ошибка обращения к БД!", result.Error.Message);
            return RedirectToAction("Available");
        }

        private IActionResult Error(string title = "Упс...", string message = "Что-то пошло не так :(")
        {
            return View("Error", new ErrorModel(title, message));
        }
    }
}