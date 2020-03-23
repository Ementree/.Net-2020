using System;
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
        public async Task<IActionResult> Index()
        {
            ViewBag.Recommendation = await _dbContext.Recommendations.FirstOrDefaultAsync();
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

            var illness = new Illness(eventVM.From, eventVM.To, HttpContext.User.Identity.Name);
            _dbContext.CalendarEntries.Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            ViewBag.Recommendation = await _dbContext.Recommendations.FirstOrDefaultAsync();
            return View(_dbContext.CalendarEntries
                .Where(c => c.AbsenceType == AbsenceType.Illness || c.AbsenceType == AbsenceType.Vacation)
                .AsEnumerable());
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCalendarEntry(int id)
        {
            var calendarEntry = await _dbContext.CalendarEntries.FindAsync(id);
            if (calendarEntry is Vacation vacation)
                vacation.Approve();
            if (calendarEntry is Illness illness)
                illness.Approve();
            await _dbContext.SaveChangesAsync();
            return RedirectToActionPermanent("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> RejectCalendarEntry(int id)
        {
            var calendarEntry = await _dbContext.CalendarEntries.FindAsync(id);
            _dbContext.CalendarEntries.Remove(calendarEntry);
            await _dbContext.SaveChangesAsync();
            return RedirectToActionPermanent("Admin");
        }

        [HttpGet]
        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateRecommendation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecommendation(Recommendation recommendation)
        {
            if (ModelState.IsValid)
            {
                var dbEntry = await _dbContext.Recommendations.FirstOrDefaultAsync();
                if (dbEntry == null)
                    await _dbContext.Recommendations.AddAsync(recommendation);
                else dbEntry.RecommendationText = recommendation.RecommendationText;
                await _dbContext.SaveChangesAsync();
                return RedirectToActionPermanent("Admin");
            }

            return RedirectToActionPermanent("UpdateRecommendation");
        }
    }
}
