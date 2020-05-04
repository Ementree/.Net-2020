﻿using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNet2020.Domain._5.Controllers
{
    public class IssueController : Controller
    {
        private readonly ILogger<IssueController> _logger;

        public IssueController(ILogger<IssueController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Show()
        {
            var issues = new Issue[]
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

            return View(new ShowIssuesModel() { Issues = issues });
        }
    }
}