using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class VacationController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public VacationController(CalendarEntryContext dbContext)
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
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var days = DomainLogic.GetDatesFromInterval(viewModel.From, viewModel.To);
            var hollidays = _dbContext.Holidays.Where(u => u.Date >= viewModel.From && u.Date <= viewModel.To).ToList();
            if (user.TotalDayOfVacation < DomainLogic.GetWorkDay(days, hollidays))
            {
                ModelState.AddModelError("Error2", "Количество запрашеваемых дней отпуска превышает количество доступных вам");
                return View(viewModel);
            }
#warning Используйте DataAnnotations аттрибуты
            if (viewModel.From == DateTime.MinValue && viewModel.From == DateTime.MinValue)
            {
                ModelState.AddModelError("Error1", "Введите даты");
                return View();
            }
            if (viewModel.From >= viewModel.To)
            {
                ModelState.AddModelError("Error", "Дата начала больничного должна быть меньше даты конца");
                return View(viewModel);
            }

            var vacation = new Vacation(viewModel.From, viewModel.To,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}