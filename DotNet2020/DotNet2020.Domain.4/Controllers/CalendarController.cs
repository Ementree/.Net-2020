using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_Models;
using Kendo.Mvc.Examples.Models.Scheduler;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            ViewBag.Recommendation = _dbContext.Recommendations.FirstOrDefault();

            var resultList = new List<CalendarEventViewModel>()
            {
                new CalendarEventViewModel()
                {
                    MeetingID = 1,
                    Title = "John Snow",
                    Description = "Soft-Engeneer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 8),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 1,
                    Attendees = new List<int> {1, 2, 3}
                },
                new CalendarEventViewModel()
                {
                    MeetingID = 2,
                    Title = "Sara James",
                    Description = "Soft-Engeneer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 7),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 2,
                    Attendees = new List<int> {1, 2, 3}
                },
                new CalendarEventViewModel()
                {
                    MeetingID = 3,
                    Title = "Susan Susan",
                    Description = "Developer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 8),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 3,
                    Attendees = new List<int> {1, 2, 3}
                }
            };

            return View(resultList);
        }

        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        #region Добавление Event в календарь

        [HttpPost]
        public IActionResult AddSickDay(EventViewModel eventVM)
        {
            if (eventVM.From == DateTime.MinValue)
            {
                ModelState.AddModelError("DataError", "Введите дату");
                return RedirectToAction("AddEvent");
            }

            var sickDay = new SickDay(eventVM.From, eventVM.From, HttpContext.User.Identity.Name);
            _dbContext.CalendarEntries.Add(sickDay);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddVacation(EventViewModel eventVM)
        {
            if (eventVM.From == DateTime.MinValue && eventVM.To == DateTime.MinValue)
            {
                ModelState.AddModelError("DataError", "Введите дату");
                return RedirectToAction("AddEvent");
            }

            if(eventVM.From >= eventVM.To)
            {
                ModelState.AddModelError("DataError", "Не корректно ведена дата");
                return RedirectToAction("AddEvent");
            }

            var vacation = new Vacation(eventVM.From, eventVM.To, HttpContext.User.Identity.Name);
            _dbContext.CalendarEntries.Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddIllness(EventViewModel eventVM)
        {
            if (eventVM.From == DateTime.MinValue && eventVM.To == DateTime.MinValue)
            {
                ModelState.AddModelError("DataError", "Введите дату");
                return RedirectToAction("AddEvent");
            }

            if (eventVM.From >= eventVM.To)
            {
                ModelState.AddModelError("DataError", "Не корректно ведена дата");
                return RedirectToAction("AddEvent");
            }

            var illness = new Illness(eventVM.From, eventVM.To, HttpContext.User.Identity.Name);
            _dbContext.CalendarEntries.Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        [HttpGet]
        public IActionResult Admin()
        {
            ViewBag.Recommendation = _dbContext.Recommendations.FirstOrDefault();
            return View(_dbContext.CalendarEntries
                .Where(c => c.AbsenceType == AbsenceType.Illness || c.AbsenceType == AbsenceType.Vacation)
                .AsEnumerable());
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var calendarEntry = _dbContext.CalendarEntries.Find(id);
            if (calendarEntry is Vacation vacation)
                vacation.Approve();
            if (calendarEntry is Illness illness)
                illness.Approve();
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var calendarEntry =  _dbContext.CalendarEntries.Find(id);
            _dbContext.CalendarEntries.Remove(calendarEntry);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpGet]
        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddHoliday(Holiday holiday)
        {
            if (!ModelState.IsValid)
                return View(holiday);

            _dbContext.Holidays.Add(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin");
        }

        [HttpGet]
        public IActionResult UpdateRecommendation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateRecommendation(Recommendation recommendation)
        {
            if (ModelState.IsValid)
            {
                var dbEntry = _dbContext.Recommendations.FirstOrDefault();
                if (dbEntry == null)
                    _dbContext.Recommendations.Add(recommendation);
                else dbEntry.RecommendationText = recommendation.RecommendationText;
                _dbContext.SaveChanges();
                return RedirectToActionPermanent("Admin");
            }

            return RedirectToActionPermanent("UpdateRecommendation");
        }
    }
}
