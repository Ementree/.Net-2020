using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
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
    public class VacationController : Controller
    {
        private readonly DbContext _dbContext;

        public VacationController(DbContext dbContext)
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
            
            var _vacation = _dbContext.Set<Vacation>()
                .FirstOrDefault(s =>
                      s.CalendarEmployeeId == employeeCalendar.Id &&
                      s.To >= viewModel.From && s.From <= viewModel.To);
            if (_vacation != null)
            {
                ModelState.AddModelError("Error", "Вы уже выбирали отпуск на эти даты, нельзя так!");
                return View(viewModel);
            }
            
            if (viewModel.IsPaid && employeeCalendar.TotalDayOfVacation < DomainLogic.GetWorkDay(days, hollidays))
            {
                ModelState.AddModelError("Error2", "Количество запрашеваемых дней отпуска превышает количество доступных вам");
                return View(viewModel);
            }

            var vacation = new Vacation(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException(),
                employeeCalendar,
                viewModel.IsPaid);
                
            _dbContext.Set<AbstractCalendarEntry>().Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}