using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using DotNet2020.Domain._5.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            var reports = ytService.GetIssues(model.Project, model.IssueFilter)
                .Where(i => i.EstimatedTime.HasValue && i.SpentTime.HasValue)
                .ToList();
            return View(reports);
        }
    }
}