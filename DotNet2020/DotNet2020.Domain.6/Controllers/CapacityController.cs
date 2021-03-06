using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain._6.Services;


namespace DotNet2020.Domain._6.Controllers
{
    public class CapacityController : Controller
    {
        private readonly DbContext _context;

        public CapacityController(DbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int year = 2020, bool withAbsence = true)
        {
            var resources = _context.Set<Resource>()
                .Include(res => res.Employee)
                .Include(x => x.ResourceGroupType)
                .ToList();

            var resourceCapacities = _context.Set<ResourceCapacity>()
                .Include(x => x.Resource)
                .Include(x => x.Period)
                .Where(x => x.Period.Start.Year == year)
                .ToList();
            var absence = _context.Set<AbstractCalendarEntry>().ToList();
            
            if (withAbsence)
            {
                var capacityWithAbsenceService = new CapacityWithAbsenceService(resourceCapacities);
                resourceCapacities = capacityWithAbsenceService.GetCapacityWithAbsence(absence);
            }
            
            var builder = new CapacityViewModelBuilder(resources, resourceCapacities, year, withAbsence);
            var model = builder.Build();
            

            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.Year = year;
            ViewBag.Months = MonthGeneratorService.GetAllMonths(year);

            return View(model);
        }

        [HttpPost("/changeCapacity")]
        public void SetCapacity([FromBody] string data)
        {
            var dataArr = data.Split(';');

            var resourceId = int.Parse(dataArr[0]);
            var month = int.Parse(dataArr[1]);
            var year = int.Parse(dataArr[2]);
            var capacity = int.Parse(dataArr[3]);
            var addCapacityService = new AddCapacityService(_context);

            var periodId = addCapacityService.GetPeriodId(month, year);
            addCapacityService.AddResourceCapacity(resourceId, periodId, capacity);

            _context.SaveChanges();
        }
    }
}