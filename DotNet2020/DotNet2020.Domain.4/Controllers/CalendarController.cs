using System;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class CalendarController : Controller
    {
        public CalendarController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
