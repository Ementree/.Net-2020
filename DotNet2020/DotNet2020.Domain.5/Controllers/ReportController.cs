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
            var model = new CreateReportModel()
            {
                CreateProject = _timeTrackingService.GetAllProjects()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            var projectName = reportResult.Result.ProjectName;

            var users = _timeTrackingService.GetAllUsers(projectName);
            if (users == null)
                return Error("Ошибка обращения к сервису трекинга!", $"Не удалось получить список пользователей проекта \"{projectName}\".");

            var usersInIssues = reportResult.Result.Issues
                .Select(i => i.AssigneeName)
                .ToHashSet();
            var selectedUsers = new bool[users.Length];
            for (int i = 0; i < users.Length; i++)
                selectedUsers[i] = usersInIssues.Contains(users[i]);

            var model = new EditReportModel()
            {
                ReportId = id,
                ProjectName = projectName,
                AllUsers = users,
                SelectedUsers = selectedUsers,
                ReportName = reportResult.Result.Name,
                IssueFilter = reportResult.Result.IssueFilter
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
            var issueFilter = model.IssueFilter == null ? "" : model.IssueFilter;
            var issues = _timeTrackingService.GetIssues(reportResult.Result.ProjectName, model.IssueFilter);

            // Change time mode
            if (model.IsWorkItems)
            {
                foreach (var issue in issues)
                    issue.SetTimeByWorkItems();
            }
            // Filter by selected users
            if (model.SelectedUsers != null)
            {
                var users = model.AllUsers
                    .Zip(model.SelectedUsers)
                    .Where(i => i.Second)
                    .Select(i => i.First)
                    .ToHashSet();
                issues = issues
                    .Where(i => users.Contains(i.AssigneeName))
                    .ToList();
            }
            // Save new report
            var newReport = new Report(reportResult.Result.Name, reportResult.Result.ProjectName, model.IssueFilter, issues);
            reportResult = _storage.EditReport(reportResult.Result.ReportId, newReport);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            // Get charts
            var charts = _chartService.GetAllCharts();
            foreach (var chart in charts.Values)
                chart.SetData(reportResult.Result.Issues, 5);

            return View("Show", new ChartModel(reportResult.Result.ReportId, charts.Values.ToList()));
        }

        [HttpPost]
        public IActionResult Show(CreateReportModel model)
        {
            if (!ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.ReportName))
                    return Error("Ошибка создания отчета!", "Название отчета должно быть не нулевым!");
                if (model.ReportName?.Length > 100)
                    return Error("Ошибка создания отчета!", "Название отчета должно быть не длиннее 100 символов!");
                if (model.IssueFilter?.Length > 1000)
                    return Error("Ошибка создания отчета!", "Фильтр должен быть не длиннее 1000 символов!");
                return Error("Ошибка создания отчета!");
            }

            RequestResult<bool> isContains = _storage.ContainsReport(model.ReportName);
            if (!isContains.IsSuccess)
                return Error("Ошибка обращения к БД!", isContains.Error.Message);
            if (isContains.Result)
                return Error("Ошибка создания отчета!", "Отчет с таким именем уже существует!");

            // Get issues
            var issues = _timeTrackingService.GetIssues(model.ProjectName, model.IssueFilter);
            if (issues == null)
                issues = new List<Issue>();

            // Save report
            var report = new Report(model.ReportName, model.ProjectName, model.IssueFilter, issues);
            _storage.SaveReport(report);

            // Get saved report with id
            var reportResult = _storage.GetReport(report.Name);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

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