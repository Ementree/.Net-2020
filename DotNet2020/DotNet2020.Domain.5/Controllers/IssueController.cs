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
        public IActionResult ShowColumn(int reportId, int start, int end, int graphId)
        {
            //var issues = new Issue[]
            //{
            //    new Issue("ADAS-81", "Нарисовать аниме", 1, null, "arsol.plex@gmail.com",
            //    "akihito.subaru@japan.jp", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-81"),

            //    new Issue("ADAS-98", "Сделать хорошее дело", null, 1, "arsol.plex@gmail.com",
            //    "arsol.plex@gmail.com", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-98"),

            //    new Issue("ADAS-99", "Сломать проект", 4, null, "arsol.plex@gmail.com",
            //    "azamat@russia.ru", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-99"),

            //    new Issue("ADAS-100", "Нарисовать аниме", 4, null, "arsol.plex@gmail.com",
            //    "somedude@mail.ru", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-100")
            //};

            var reportResult = _storage.GetReport(reportId);
            if (!reportResult.IsSuccess)
                return RedirectToAction("Error");

            var chart = _chartService.GetChart(graphId);
            chart.SetData(reportResult.Result.Issues, 6);
            var issues = chart.GetIssues(start, end);
            return View("Show", new ShowIssuesModel() { Issues = issues });
        }

        [HttpPost]
        public IActionResult Show()
        {
            var issues = new List<Issue>
            {
                new Issue("ADAS-81", "Нарисовать аниме", 1, null, "arsol.plex@gmail.com",
                "akihito.subaru@japan.jp", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-81"),

                new Issue("ADAS-98", "Сделать хорошее дело", null, 1, "arsol.plex@gmail.com",
                "arsol.plex@gmail.com", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-98"),

                new Issue("ADAS-99", "Сломать проект", 4, null, "arsol.plex@gmail.com",
                "azamat@russia.ru", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-99"),

                new Issue("ADAS-100", "Нарисовать аниме", 4, null, "arsol.plex@gmail.com",
                "somedude@mail.ru", "ADAS", @"https://kpfu-net.myjetbrains.com/youtrack/issue/ADAS-100")
            };

            return View("Show", new ShowIssuesModel() { Issues = issues });
        }
    }
}