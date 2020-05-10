using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Core.Models;
using DotNet2020.Domain.Models;
using DotNet2020.Domain.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class CalendarController : Controller
    {
        private readonly DbContext _dbContext;

        public CalendarController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var emp = _dbContext.Set<Employee>().ToList();
            var employee = _dbContext.Set<AppIdentityUser>()
                .FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Employee;
            var user = _dbContext.Set<EmployeeCalendar>()
                .FirstOrDefault(u => u.Employee == employee);
            ViewBag.TotalVacation = user?.TotalDayOfVacation;
            ViewBag.Recommendation = _dbContext.Set<Recommendation>().FirstOrDefault();
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
    }
}
