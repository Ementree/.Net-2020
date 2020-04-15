﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Domain._5.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNet2020.Controllers
{
    public class ReportViewController : Controller
    {
        private readonly ILogger<ReportViewController> _logger;

        public ReportViewController(ILogger<ReportViewController> logger)
        {
            _logger = logger;
        }

        public IActionResult Report()
        {
            var reports = new List<IssueTimeInfo>
                {
                    new IssueTimeInfo(10,7),
                    new IssueTimeInfo(11, 8),
                    new IssueTimeInfo(15,1),
                    new IssueTimeInfo(20,19),
                    new IssueTimeInfo(25, 7)
                };
            return View(reports);
        }

    }
}
