using System;
using System.Linq;
using DotNet2020.Data;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using DotNet2020.Domain.Filters;
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
            // #warning Используйте DataAnnotations аттрибуты
            // if (viewModel.Day == DateTime.MinValue)
            // {
            //     ModelState.AddModelError("Error1", "Введите дату");
            //     return View();
            // }
            var sickDay = new SickDay(
                viewModel.Day ?? throw new NullReferenceException(),
                viewModel.Day ?? throw new NullReferenceException(),
                _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.Set<AbstractCalendarEntry>().Add(sickDay);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}