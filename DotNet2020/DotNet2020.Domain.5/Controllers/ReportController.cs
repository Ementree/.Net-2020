using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Domain._5.Models;
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

        public IActionResult CreateReport()
        {
            return View();
        }
    }
}
