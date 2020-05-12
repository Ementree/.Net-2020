using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Controllers
{
    public class ResourceController : Controller
    {
        private readonly DbContext _context;

        public ResourceController(DbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var resources = _context.Set<Resource>()
                .Include(r => r.Employee)
                .Include(r => r.ResourceGroupType)
                .ToList();
            
            
            return View(resources);
        }
    }
}