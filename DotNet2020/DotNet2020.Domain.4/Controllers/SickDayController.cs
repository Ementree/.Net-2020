using System;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class SickDayController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public SickDayController(CalendarEntryContext dbContext)
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
        public IActionResult Add(SickDayViewModel viewModel)
        {
            #warning Используйте DataAnnotations аттрибуты
            if (viewModel.Day == DateTime.MinValue)
            {
                ModelState.AddModelError("Error1", "Введите дату");
                return View();
            }
            var sickDay = new SickDay(viewModel.Day, viewModel.Day,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(sickDay);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}