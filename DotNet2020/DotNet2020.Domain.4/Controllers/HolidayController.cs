using System;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    [Authorize]
    public class HolidayController : Controller
    {
        private readonly DbContext _dbContext;

        public HolidayController(DbContext dbContext)
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
        public IActionResult Add(Holiday holiday)
        {
            _dbContext.Set<Holiday>().Add(holiday);
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Index", "AdminCalendar");
        }

        [HttpGet]
        public IActionResult Remove()
        {
            return View(_dbContext.Set<Holiday>().ToList());
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var holiday = _dbContext.Set<Holiday>().FirstOrDefault(u => u.Id == id);
            _dbContext.Set<Holiday>().Remove(holiday ?? throw new NullReferenceException());
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Remove");
        }
    }
}