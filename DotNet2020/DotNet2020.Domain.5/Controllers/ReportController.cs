using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNet2020.Controllers
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
            return View();
        }

        [HttpPost]
        public IActionResult Show(CreateReportModel model)
        {
            var reports = new List<IssueTimeInfo>
                {
                    new IssueTimeInfo(10,7),
                    new IssueTimeInfo(11, 8),
                    new IssueTimeInfo(15, 12),
                    new IssueTimeInfo(20,19),
                    new IssueTimeInfo(25, 7),
                    new IssueTimeInfo(10, 17),
                    new IssueTimeInfo(11, 18),
                    new IssueTimeInfo(55, 52),
                    new IssueTimeInfo(20,19),
                    new IssueTimeInfo(35, 17),
                    new IssueTimeInfo(10, 17),
                    new IssueTimeInfo(11, 8),
                    new IssueTimeInfo(15,12),
                    new IssueTimeInfo(21,12),
                    new IssueTimeInfo(25, 17)
                };
            return View(reports);
        }
    }
}
