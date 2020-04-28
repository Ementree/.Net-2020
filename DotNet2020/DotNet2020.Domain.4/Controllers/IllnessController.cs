using System;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._4_.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class IllnessController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public IllnessController(CalendarEntryContext dbContext)
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
            var illness = new Illness(viewModel.From, viewModel.To,
                _dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.CalendarEntries.Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}