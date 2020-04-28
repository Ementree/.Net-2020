using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class HolidayController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public HolidayController(CalendarEntryContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #warning выделить в отдельный контроллер
        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidationFilter]
        public IActionResult Add(Holiday holiday)
        {
            _dbContext.Holidays.Add(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin", "Calendar");
        }

        [HttpGet]
        public IActionResult Remove()
        {
            ViewBag.Holidays = _dbContext.Holidays.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult RemoveHolidays(int id)
        {
            var holiday = _dbContext.Holidays.FirstOrDefault(u => u.Id == id);
            _dbContext.Holidays.Remove(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Remove");
        }

        public ActionResult GetHolidays()
        {
            // logic
            // Edit you don't need to serialize it just return the object
            var result = _dbContext.Holidays
                .ToList()
                .Select(u =>
                {
                    var year = u.Date.Year.ToString();
                    var month = u.Date.Month.ToString().StartsWith('0') ? u.Date.Month.ToString().Skip(1) : u.Date.Month.ToString();
                    var day = u.Date.Day.ToString().StartsWith('0') ? u.Date.Day.ToString().Skip(1) : u.Date.Day.ToString();
                    return $"{year}/{month}/{day}";
                })
                .ToList();

            return Json(result);
        }
    }
}