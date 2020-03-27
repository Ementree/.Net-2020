﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_Models;
using DotNet2020.Domain.Models;
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
        public IActionResult Index()
        {
            ViewBag.Recommendation = _dbContext.Recommendations.FirstOrDefault();
            return View();
        }

        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        #region Добавление Event в календарь

        [HttpPost]
        public IActionResult AddSickDay(EventVM eventVM)
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
        public IActionResult AddVacation(EventVM eventVM)
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
        public IActionResult AddIllness(EventVM eventVM)
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
