using System.Linq;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class AdminCalendarController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public AdminCalendarController(CalendarEntryContext dbContext)
        {
            _dbContext = dbContext;
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
            #warning Добавить интерфейс, заменить на полиморфизм
            var calendarEntry = _dbContext.CalendarEntries.Find(id);            
            if (calendarEntry is Vacation vacation)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id == calendarEntry.UserId);
                var hollidays = _dbContext.Holidays.Where(u => u.Date >= calendarEntry.From && u.Date <= calendarEntry.To).ToList();
                var days = DomainLogic.GetDatesFromInterval(calendarEntry.From, calendarEntry.To);
                var total = DomainLogic.GetWorkDay(days, hollidays);
                user.TotalDayOfVacation = user.TotalDayOfVacation - total;
                #warning Добавить User в качестве согласующего
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
    }
}