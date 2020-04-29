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
    public class IllnessController : Controller
    {
        private readonly DbContext _dbContext;

        public IllnessController(DbContext dbContext)
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
            // #warning Используйте DataAnnotations аттрибуты
            // if (viewModel.From == DateTime.MinValue && viewModel.From == DateTime.MinValue)
            // {
            //     ModelState.AddModelError("Error1", "Введите даты");
            //     return View();
            // }
            // if (viewModel.From >= viewModel.To)
            // {
            //     ModelState.AddModelError("Error", "Дата начала больничного должна быть меньше даты конца");
            //     return View(viewModel);
            // }
            var illness = new Illness(
                viewModel.From ?? throw new NullReferenceException(), 
                viewModel.To ?? throw new NullReferenceException(), 
                _dbContext.Set<AppIdentityUser>().FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name));
            _dbContext.Set<AbstractCalendarEntry>().Add(illness);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Calendar");
        }
    }
}