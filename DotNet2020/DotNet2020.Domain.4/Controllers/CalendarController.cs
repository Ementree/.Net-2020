using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using DotNet2020.Domain.Filters;
using DotNet2020.Domain.Models.ModelView;
using Kendo.Mvc.Examples.Models.Scheduler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class CalendarController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public CalendarController(CalendarEntryContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.TotalVacation = user.TotalDayOfVacation;
            ViewBag.Recommendation = _dbContext.Recommendations.FirstOrDefault();
            ViewBag.User = user;

            var allVacations = _dbContext.GetAllVacations();
            var users = _dbContext.GetAllUsers();
            var holidays = _dbContext.GetAllHolidays();

            return View(new IndexViewModel() { Events = allVacations, Users = users, Holidays=holidays });
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddEvent()
        {

            return View();
        }

        #region Добавление Event в календарь

        [HttpGet]
        [Authorize]
        public IActionResult AddSickDay()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSickDay(SickDayViewModel viewModel)
        {
            if(viewModel.Day == DateTime.MinValue)
            {
                ModelState.AddModelError("Error1", "Введите дату");
                return View();
            }
            var sickDay = new SickDay(viewModel.Day, viewModel.Day,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(sickDay);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddVacation()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddVacation(VacationViewModel viewModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var days = GetDatesFromInterval(viewModel.From, viewModel.To);
            var hollidays = _dbContext.Holidays.Where(u => u.Date >= viewModel.From && u.Date <= viewModel.To).ToList();
            if (user.TotalDayOfVacation < GetWorkDay(days, hollidays))
            {
                ModelState.AddModelError("Error2", "Количество запрашеваемых дней отпуска превышает количество доступных вам");
                return View(viewModel);
            }
            if (viewModel.From == DateTime.MinValue && viewModel.From == DateTime.MinValue)
            {
                ModelState.AddModelError("Error1", "Введите даты");
                return View();
            }
            if (viewModel.From >= viewModel.To)
            {
                ModelState.AddModelError("Error", "Дата начала больничного должна быть меньше даты конца");
                return View(viewModel);
            }

            var vacation = new Vacation(viewModel.From, viewModel.To,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddIllness()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddIllness(VacationViewModel viewModel)
        {
            if (viewModel.From == DateTime.MinValue && viewModel.From == DateTime.MinValue)
            {
                ModelState.AddModelError("Error1", "Введите даты");
                return View();
            }
            if (viewModel.From >= viewModel.To)
            {
                ModelState.AddModelError("Error", "Дата начала больничного должна быть меньше даты конца");
                return View(viewModel);
            }
            var illness = new Illness(viewModel.From, viewModel.To,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion
        public List<DateTime> GetDatesFromInterval(DateTime startDate, DateTime endDate)
        {
            List<DateTime> result = new List<DateTime>();
            if (startDate > endDate) return result;
            result.Add(startDate);
            while (startDate < endDate)
            {
                DateTime d = startDate.AddDays(1);
                result.Add(d);
                startDate = d;
            }
            return result;
        }

        public int GetWorkDay(List<DateTime> days, List<Holiday> hollidays)
        {
            int total = 0;
            foreach (var day in days)
            {
                bool flag = true;
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    foreach (var holliday in hollidays)
                    {
                        if (day == holliday.Date)
                            flag = false;
                        continue;
                    }
                    if (flag)
                        total++;
                }
            }
            return total;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Admin()
        {
            ViewBag.Recommendation = _dbContext.Recommendations.FirstOrDefault();
            ViewBag.Events = _dbContext.CalendarEntries
                .Where(c => c.AbsenceType == AbsenceType.Illness || c.AbsenceType == AbsenceType.Vacation)
                .Include(m => m.User)
                .AsEnumerable();

            var allVacations = _dbContext.GetAllVacations();
            var users = _dbContext.GetAllUsers();
            var holidays = _dbContext.GetAllHolidays();

            return View(new IndexViewModel() { Events = allVacations, Users = users, Holidays = holidays });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Approve(int id)
        {
            var calendarEntry = _dbContext.CalendarEntries.Find(id);            
            if (calendarEntry is Vacation vacation)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == calendarEntry.UserId);
                var hollidays = _dbContext.Holidays.Where(u => u.Date >= calendarEntry.From && u.Date <= calendarEntry.To).ToList();
                var days = GetDatesFromInterval(calendarEntry.From, calendarEntry.To);
                var total = GetWorkDay(days, hollidays);
                user.TotalDayOfVacation = user.TotalDayOfVacation - total;
                vacation.Approve();
                user.Approve();
            }
            if (calendarEntry is Illness illness)
                illness.Approve();
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Reject(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user != null)
                user.Reject();
            var calendarEntry =  _dbContext.CalendarEntries.Find(id);
            _dbContext.CalendarEntries.Remove(calendarEntry);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidationFilter]
        public IActionResult AddHoliday(Holiday holiday)
        {
            _dbContext.Holidays.Add(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdateRecommendation()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidationFilter]
        public IActionResult UpdateRecommendation(Recommendation recommendation)
        {
            var dbEntry = _dbContext.Recommendations.FirstOrDefault();
            if (dbEntry == null)
                _dbContext.Recommendations.Add(recommendation);
            else dbEntry.RecommendationText = recommendation.RecommendationText;
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }


        [HttpGet]
        public IActionResult RemoveHoliday()
        {
            ViewBag.Holidays = _dbContext.Holidays.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult RemoveHolidays(int id)
        {
            var holiday = _dbContext.Holidays.Where(u => u.Id == id).FirstOrDefault();
            _dbContext.Holidays.Remove(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("RemoveHoliday");
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
