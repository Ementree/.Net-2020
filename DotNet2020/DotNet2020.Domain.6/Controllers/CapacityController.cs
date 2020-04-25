using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain._6.Models.ViewModels;
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

        public IActionResult Index(int year = 2020)
        {
            var resources = _context.Set<Resource>()
                .Include(x => x.ResourceGroupType)
                .ToList();
            
            var resourceCapacities = _context.Set<ResourceCapacity>()
                .Include(x => x.Resource)
                .Include(x => x.Period)
                .Where(x => x.Period.Start.Year == year)
                .ToList();

            var builder = new CapacityViewModelBuilder(resources, resourceCapacities);
            var model = builder.Build();

            ViewBag.CurrentYear = DateTime.Now.Year;
            ViewBag.Year = year;
            ViewBag.Months = MonthGeneratorService.GetAllMonths(year);

            return View(model);
        }

        [HttpPost("/changeCapacity")]
        public void ChangeCapacity([FromBody] string data)
        {
            var dataArr = data.Split(';');

            var resourceId = int.Parse(dataArr[0]);
            var month = int.Parse(dataArr[1]);
            var year = int.Parse(dataArr[2]);
            var capacity = int.Parse(dataArr[3]);

            var resources = _context.Set<Resource>()
                .Include(x => x.ResourceGroupType)
                .ToList();
            var resourceCapacities = _context.Set<ResourceCapacity>();
            var periods = _context.Set<Period>();

            var periodId = 0;
            var period = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year);
            if (period == null)
            {
                periods.Add(new Period(new DateTime(year, month, 1), new DateTime(year, month, 28)));
                _context.SaveChanges();

                periodId = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year).Id;
                _context.SaveChanges();
            }
            else
            {
                periodId = period.Id;
            }

            var resourceCapacity =
                resourceCapacities.FirstOrDefault(res => res.ResourceId == resourceId && res.PeriodId == periodId);
            var newResourceCapacity = new ResourceCapacity(resourceId, capacity, periodId);

            if (resourceCapacity == null)
            {
                resourceCapacities.Add(new ResourceCapacity(resourceId, capacity, periodId));
            }
            else
            {
                resourceCapacity.Capacity = capacity;
                resourceCapacities.Update(resourceCapacity);
            }

            _context.SaveChanges();
        }
    }
}