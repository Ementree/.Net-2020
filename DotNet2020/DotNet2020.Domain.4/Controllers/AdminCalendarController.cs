using System;
using System.Linq;
using DotNet2020.Data;
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
            var user = _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            if (user != null)
                user.Reject();
            var calendarEntry =  _dbContext.Set<AbstractCalendarEntry>().Find(id);
            _dbContext.Set<AbstractCalendarEntry>().Remove(calendarEntry);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Index");
        }
    }
}