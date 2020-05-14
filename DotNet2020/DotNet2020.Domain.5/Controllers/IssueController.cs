using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;
        private readonly IStorage _storage;
        private readonly ITimeTrackingService _timeTrackingService;
        private readonly IChartService _chartService;

        public IssueController(ILogger<IssueController> logger, DbContext db)
        {
            _logger = logger;
            _storage = new Storage(db);
            _timeTrackingService = new YouTrackService();
            _chartService = new ChartService();
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
            return View("Show", new ShowIssuesModel() { Issues = issues });
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            var reportResult = _storage.GetReport(id);
            if (!reportResult.IsSuccess)
                return Error("Ошибка обращения к БД!", reportResult.Error.Message);

            var issues = reportResult.Result.Issues;

            return View("Show", new ShowIssuesModel() { Issues = issues });
        }

        private IActionResult Error(string title = "Упс...", string message = "Что-то пошло не так :(")
        {
            return View("Error", new ErrorModel(title, message));
        }
    }
}