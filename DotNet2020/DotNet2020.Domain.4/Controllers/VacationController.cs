using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
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
            var user = _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var days = DomainLogic.GetDatesFromInterval(viewModel.From, viewModel.To);
            var hollidays = _dbContext.Set<Holiday>().Where(u => u.Date >= viewModel.From && u.Date <= viewModel.To).ToList();
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
                _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.Set<AbstractCalendarEntry>().Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}