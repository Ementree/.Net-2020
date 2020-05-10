using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using DotNet2020.Domain.Filters;
using DotNet2020.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class SickDayController : Controller
    {
        private readonly DbContext _dbContext;

        public SickDayController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        [ValidationFilter]
        public IActionResult Add(SickDayViewModel viewModel)
        {
            var employee = _dbContext.Set<AppIdentityUser>()
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Employee;
            var employeeCalendar = _dbContext.Set<EmployeeCalendar>()
                .FirstOrDefault(u => u.Employee == employee);
            var sickday = _dbContext.Set<SickDay>()
                .FirstOrDefault(s => s.From == viewModel.Day && s.CalendarEmployeeId == employeeCalendar.Id);
            if (sickday != null)
            {
                ModelState.AddModelError("Error", "Вы уже выбирали sickDay на эту дату, нельзя так!");
                return View(viewModel);
            }

            var sickDay = new SickDay(
                viewModel.Day ?? throw new NullReferenceException(),
                viewModel.Day ?? throw new NullReferenceException(),
                employeeCalendar);
            _dbContext.Set<AbstractCalendarEntry>().Add(sickDay);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}