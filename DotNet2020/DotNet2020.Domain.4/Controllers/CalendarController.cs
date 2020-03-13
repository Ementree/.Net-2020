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

        public IActionResult AddEvent()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult AddHoliday()
        {
            return View();
        }

        public IActionResult AddRecommendation()
        {
            return View();
        }
    }
}
