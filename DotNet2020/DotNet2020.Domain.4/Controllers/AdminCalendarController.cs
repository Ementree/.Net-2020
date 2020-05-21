using System;
using System.Linq;
using System.Security.Claims;
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
    [Authorize(Roles = "admin")]
    public class AdminCalendarController : Controller
    {
        private readonly DbContext _dbContext;

        public AdminCalendarController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Recommendation = _dbContext.Set<Recommendation>().FirstOrDefault();
            ViewBag.Events = _dbContext.Set<AbstractCalendarEntry>()
                .Where(c => c.AbsenceType == AbsenceType.Illness || c.AbsenceType == AbsenceType.Vacation)
                .Include(m => m.CalendarEmployee)
                .ToList();

            var allVacations = _dbContext.GetAllVacations();
            var users = _dbContext.GetAllUsers();
            var holidays = _dbContext.GetAllHolidays();

            return View(new IndexViewModel() { Events = allVacations, Users = users, Holidays = holidays });
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var calendarEntry = _dbContext.Set<AbstractCalendarEntry>()
                .Include(u => u.CalendarEmployee)
                .FirstOrDefault(u => u.Id == id);
            if (calendarEntry is IApprovableEvent approvableEvent)
            {
                var employee = _dbContext.Set<AppIdentityUser>()
                            .Include(u => u.Employee)
                            .FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Employee;
                approvableEvent
                    .Approve(_dbContext.Set<Holiday>()
                    .Where(u => u.Date >= calendarEntry.From &&
                                u.Date <= calendarEntry.To).ToList(),
                            employee);
                _dbContext.SaveChanges();
            }
            else throw new ArgumentException("You trying to Approve non approvable entry");
            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            var calendarEntry =  _dbContext.Set<AbstractCalendarEntry>().Find(id);
            var user = _dbContext.Set<EmployeeCalendar>().FirstOrDefault(u => u.Id == calendarEntry.CalendarEmployeeId);
            if (calendarEntry is Vacation vacation && vacation.IsPaid)
                user.TotalDayOfVacation += (calendarEntry.To - calendarEntry.From).Days + 1;
            user?.Reject();
            _dbContext.Set<AbstractCalendarEntry>().Remove(calendarEntry);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Index");
        }

        [HttpPost]
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
            foreach (var user in _dbContext.Set<EmployeeCalendar>().ToList())
                user.TotalDayOfVacation += 28;
            _dbContext.Set<YearOfVacations>().Add(new YearOfVacations { Year = DateTime.Now.Year });
            _dbContext.SaveChanges();
        }
    }
}