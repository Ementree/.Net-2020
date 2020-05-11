using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Services;
using DotNet2020.Domain._6.Services.Absences;

namespace DotNet2020.Domain._6.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly DbContext _context;
        public AbsenceController(DbContext context)
        {
            this._context = context;
        }
        // GET
        public IActionResult Index(int year = 2020)
        {
            ViewBag.Year = year;

            var absences = _context.Set<AbstractCalendarEntry>().ToList();

            var resourcesCapacity = _context.Set<ResourceCapacity>()
                .Include(res => res.Resource)
                .ThenInclude(res => res.Employee)
                .ToList();
            
            var functioningCapacityResources = _context.Set<FunctioningCapacityResource>()
                .Include(res => res.Resource)
                .ThenInclude(res => res.Employee).ToList();
            
            var resources = _context.Set<Resource>()
                .Include(res => res.Employee)
                .Include(res=>res.ResourceGroupType)
                .ToList();
            
            var periods = _context.Set<Period>().Where(per => per.Start.Year == year).OrderBy(per => per.Start).ToList();
            
            
            var builder = new AbsencesViewModelBuilder(year,absences,resourcesCapacity,functioningCapacityResources,resources,periods);
            var model = builder.Build();
            ViewBag.Months = MonthGeneratorService.GetAllMonths(year);

            return View(model);
        }


    }  
}