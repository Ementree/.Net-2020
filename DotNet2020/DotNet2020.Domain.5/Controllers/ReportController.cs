using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DotNet2020.Domain._5.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;

        public ReportController(ILogger<ReportController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateReportModel()
            {
                CreateProject = new string[]
            {"option 1", "option 2", "option 3", "option 4",  "option 5", "option 6"}
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var model = new EditReportModel()
            {
                ProjectName = "option 1",
                UserFilter = new string[]
                {"option 1", "option 2", "option 3", "option 4",  "option 5", "option 6"}
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Show(CreateReportModel model)
        {
            var ytService = new YouTrackService();
            var reports = ytService.GetIssuesTimeInfo(model.ProjectName, model.IssueFilter)
                .Where(i => i.EstimatedTime.HasValue && i.SpentTime.HasValue)
                .ToList();
            return View(reports);

            //var estChart = new Chart(issues.OrderBy(t => t.TimeInfo.EstimatedTime).Select(t => (double)t.TimeInfo.EstimatedTime.Value), Count);
            //var spentChart = new Chart(issues.OrderBy(t => t.TimeInfo.SpentTime).Select(t => (double)t.TimeInfo.SpentTime.Value), Count);
            //var erCoefChart = new Chart(issues.OrderBy(t => t.TimeInfo.GetErrorCoef()).Select(t => t.TimeInfo.GetErrorCoef().Value), Count);
            //var erChart = new Chart(issues.OrderBy(t => t.TimeInfo.GetErrorHours()).Select(t => (double)t.TimeInfo.GetErrorHours().Value), Count);
            //return View(new ChartModel(new List<Chart>() { estChart, spentChart, erCoefChart, erChart }, issues));
        }
    }
}