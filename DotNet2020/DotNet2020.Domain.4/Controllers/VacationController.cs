using System;
using System.Linq;
using System.Security.Claims;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using DotNet2020.Domain.Filters;
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
        [ValidationFilter]
        public IActionResult Add(VacationViewModel viewModel)
        {
            var user = _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => 
                u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var days = DomainLogic.GetDatesFromInterval(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException());
            
            var hollidays = _dbContext.Set<Holiday>().Where(u => 
                u.Date >= viewModel.From && u.Date <= viewModel.To).ToList();
            
            #warning Используйте DataAnnotations аттрибуты
            if (user.TotalDayOfVacation < DomainLogic.GetWorkDay(days, hollidays))
            {
                ModelState.AddModelError("Error2", "Количество запрашеваемых дней отпуска превышает количество доступных вам");
                return View(viewModel);
            }

            var vacation = new Vacation(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException(),
                _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.Set<AbstractCalendarEntry>().Add(vacation);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}