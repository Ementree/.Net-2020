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
            var ytService = new YouTrackService();
            var model = new CreateReportModel()
            {
                CreateProject = ytService.GetAllProjects()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var ytService = new YouTrackService();
            var model = new EditReportModel()
            {
                ProjectName = "option 1",
                UserFilter = ytService.GetAllProjects()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Show(CreateReportModel model)
        {
            var ytService = new YouTrackService();
            var reports = ytService.GetIssues(model.ProjectName, model.IssueFilter)
                .Where(i => i.EstimatedTime.HasValue && i.SpentTime.HasValue)
                .ToList();
            return View(reports);
        }
    }
}