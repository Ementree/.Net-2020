using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using DotNet2020.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class VacationController : Controller
    {
        private readonly DbContext _dbContext;

        public VacationController(DbContext dbContext)
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
        public IActionResult Add(VacationViewModel viewModel)
        {
            var employee = _dbContext.Set<AppIdentityUser>()
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Employee;
            var employeeCalendar = _dbContext.Set<EmployeeCalendar>()
                .FirstOrDefault(u => u.Employee == employee);

            var days = DomainLogic.GetDatesFromInterval(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException());
            
            var hollidays = _dbContext.Set<Holiday>().Where(u => 
                u.Date >= viewModel.From && u.Date <= viewModel.To).ToList();
            
            #warning Используйте DataAnnotations аттрибуты
            if (employeeCalendar.TotalDayOfVacation < DomainLogic.GetWorkDay(days, hollidays))
            {
                ModelState.AddModelError("Error2", "Количество запрашеваемых дней отпуска превышает количество доступных вам");
                return View(viewModel);
            }
            if (viewModel.From >= viewModel.To)
            {
                ModelState.AddModelError("Error", "Дата начала больничного должна быть меньше даты конца");
                return View(viewModel);
            }

            var vacation = new Vacation(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException(),
                employeeCalendar);
            _dbContext.Set<AbstractCalendarEntry>().Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}