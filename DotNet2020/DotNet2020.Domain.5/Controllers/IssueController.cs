using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;
        private readonly IStorage _storage;
        private readonly ITimeTrackingService _timeTrackingService;
        private readonly IChartService _chartService;

        public IssueController(IConfiguration configuration, ILogger<IssueController> logger, DbContext db)
        {
            _logger = logger;
            _storage = new Storage(db);
            _timeTrackingService = new YouTrackService(configuration);
            _chartService = new ChartService();
        }

        [HttpGet]
        public IActionResult ShowProblematic(int id)
        {
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            var issues = _timeTrackingService.GetProblemIssues(reportResult.Result.Issues);

            return View("Show", new ShowIssuesModel() { Issues = issues, ReportId = id });
        }

        [HttpGet]
        public IActionResult ShowRange(int reportId, int start, int end, int graphId)
        {
            var reportResult = _storage.GetReport(reportId);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            var chart = _chartService.GetChart(graphId);
            chart.SetData(reportResult.Result.Issues, 6);
            var issues = chart.GetIssues(start, end);
            return View("Show", new ShowIssuesModel() { Issues = issues, ReportId = reportId });
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            // Filter issues
            var issuesToShow = reportResult.Result.Issues
                .Where(i => i.SpentTime.HasValue && i.EstimatedTime.HasValue)
                .ToList();

            return View("Show", new ShowIssuesModel() { Issues = issuesToShow, ReportId = id });
        }

        [HttpPost]
        public IActionResult Show(ShowIssuesModel model)
        {
            if (String.IsNullOrEmpty(model.SerializedIssues))
                return View("Show", new ShowIssuesModel());

            // Set custom settings (to be able to use private setters)
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new JsonContractResolver()
            };

            // Deserialize issues
            var issues = JsonConvert.DeserializeObject<List<Issue>>(model.SerializedIssues, settings);
            model.Issues = issues;
            if (String.IsNullOrEmpty(model.OrderBy) && String.IsNullOrEmpty(model.OrderByDescending))
                return View("Show", model);

            // Get property to order by
            var property = typeof(Issue).GetProperty(String.IsNullOrEmpty(model.OrderBy) ? model.OrderByDescending : model.OrderBy);
            if (property == null)
                return View("Show", model);

            // Order by property
            if (!String.IsNullOrEmpty(model.OrderBy))
                issues = model.Issues.OrderBy(i => property.GetValue(i)).ToList();
            else if (!String.IsNullOrEmpty(model.OrderByDescending))
                issues = model.Issues.OrderByDescending(i => property.GetValue(i)).ToList();

            return View("Show", new ShowIssuesModel() { Issues = issues, ReportId = model.ReportId });
        }

        private IActionResult Error(string title = "Упс...", string message = "Что-то пошло не так :(")
        {
            return View("Error", new ErrorModel(title, message));
        }
    }
}