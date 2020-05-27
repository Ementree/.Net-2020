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
    [Authorize]
    public class IllnessController : Controller
    {
        private readonly DbContext _dbContext;

        public IllnessController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidationFilter]
        public IActionResult Add(IllnessViewModel viewModel)
        {
            var employee = _dbContext.Set<AppIdentityUser>()
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Employee;
            var employeeCalendar = _dbContext.Set<EmployeeCalendar>()
                .FirstOrDefault(u => u.Employee == employee);

            var events = _dbContext.Set<AbstractCalendarEntry>()
                .FirstOrDefault(s =>
                      s.CalendarEmployeeId == employeeCalendar.Id &&
                      s.To >= viewModel.From && s.From <= viewModel.To);
            if (events != null)
            {
                ModelState.AddModelError("Error", "Выбранная вами дата пересекается с уже имеющимися в календаре событиями");
                return View(viewModel);
            }

            var illness = new Illness(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException(),
                employeeCalendar);
                
            _dbContext.Set<AbstractCalendarEntry>().Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}