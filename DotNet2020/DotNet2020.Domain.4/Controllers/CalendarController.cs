using System;
using System.Threading.Tasks;
using DotNet2020.Data;
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
        public IActionResult AddSeekday(EventVM eventVM)
        {
            if (eventVM.From == DateTime.MinValue)
            {
                ModelState.AddModelError("DataError", "Введите дату");
                return RedirectToAction("AddEvent");
            }
            var seekday = new SickDay(eventVM.From, eventVM.From);
            _dbContext.SickDays.Add(seekday);
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
            var vacation = new Vacation(eventVM.From, eventVM.To);
            _dbContext.Vacations.Add(vacation);
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
            var illness = new Illness(eventVM.From, eventVM.To);
            _dbContext.Illnesses.Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        public async Task<IActionResult> Admin()
        {
            ViewBag.Recommendation = await _dbContext.Recommendations.FirstOrDefaultAsync();
            return View();
        }

        [HttpGet]
        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRecommendation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecommendation(Recommendation recommendation)
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

            return RedirectToActionPermanent("AddRecommendation");
        }
    }
}
