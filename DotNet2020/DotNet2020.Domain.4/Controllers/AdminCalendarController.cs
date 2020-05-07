using System;
using System.Linq;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Models;
using DotNet2020.Domain.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class AdminCalendarController : Controller
    {
        private readonly DbContext _dbContext;

        public AdminCalendarController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Recommendation = _dbContext.Set<Recommendation>().FirstOrDefault();
            ViewBag.Events = _dbContext.Set<AbstractCalendarEntry>()
                .Where(c => c.AbsenceType == AbsenceType.Illness || c.AbsenceType == AbsenceType.Vacation)
                .Include(m => m.User)
                .ToList();

            var allVacations = _dbContext.GetAllVacations();
            var users = _dbContext.GetAllUsers();
            var holidays = _dbContext.GetAllHolidays();

            return View(new IndexViewModel() { Events = allVacations, Users = users, Holidays = holidays });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Approve(int id)
        {
            var calendarEntry = _dbContext.Set<AbstractCalendarEntry>().Find(id);
            if(calendarEntry is IApprovableEvent approvableEvent)
                approvableEvent.Approve(_dbContext);
            else throw new ArgumentException("You trying to Approve non approvable entry");
            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Reject(int id)
        {
            var calendarEntry =  _dbContext.Set<AbstractCalendarEntry>().Find(id);
            var user = _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Id == calendarEntry.UserId);
            user?.Reject();
            _dbContext.Set<AbstractCalendarEntry>().Remove(calendarEntry);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Refresh()
        {
            var currentYear = _dbContext.Set<YearOfVacations>().FirstOrDefault(m => m.Year == DateTime.Now.Year);
            if (currentYear == null)
            {
                RefreshTotalDayOfVacations();
                TempData["message"] = string.Format("Количество оплачиваемых дней отпуска успешно обновлен.");
            }
            else
                TempData["message"] = string.Format("На текущий год количество оплачиваемых дней отпуска уже обновлен.");

            return RedirectToActionPermanent("Index");
        }

        public void RefreshTotalDayOfVacations()
        {
            foreach (var user in _dbContext.Set<AppIdentityUser>().ToList())
                user.TotalDayOfVacation += 28;
            _dbContext.Set<YearOfVacations>().Add(new YearOfVacations { Year = DateTime.Now.Year });
            _dbContext.SaveChanges();
        }
    }
}