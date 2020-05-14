using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet2020.Data;
using DotNet2020.Domain.Core.Models;
using DotNet2020.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotNet2020.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContext _context;

        public HomeController(ILogger<HomeController> logger, DbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
              var user = _context.Set<AppIdentityUser>()
                  .Include(u => u.Employee)
                  .FirstOrDefault(u => User.Identity.Name == u.Email);
              if (user == default) throw new Exception("User is null");
              ViewBag.HasEmployee = user.Employee != null;              
            }

            return View();
        }
        
        public IActionResult BindWithEmployee()
        {
            var employee = _context.Set<Employee>().FirstOrDefault(e => e.Email == User.Identity.Name);
            if (employee == default)
            {
                return RedirectToAction("WorkersAdd", "Attestation");
            }
            else
            {
                var user = _context.Set<AppIdentityUser>()
                    .Include(u => u.Employee)
                    .FirstOrDefault(u => User.Identity.Name == u.Email);
                user.Employee = employee;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
