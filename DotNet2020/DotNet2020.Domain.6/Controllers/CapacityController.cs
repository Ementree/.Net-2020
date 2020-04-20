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
        private DbContext context;

        public CapacityController(DbContext _context)
        {
            context = _context;
        }

        public IActionResult Index(int year = 2020)
        {
            var resources = context.Set<Resource>()
                .Include(x => x.ResourceGroupType)
                .ToList();
            var resourceCapacities = context.Set<ResourceCapacity>()
                .Include(x => x.Resource)
                .Include(x => x.Period)
                .Where(x => x.Period.Start.Year == year)
                .ToList();

            var viewModelList = new List<ViewModelCapacity>();
            var capacities = resourceCapacities
                .GroupBy(res => res.Resource.Id)
                .Select(x => new
                {
                    resourceId = x.Key,
                    capacity = x.ToDictionary(y => y.Period.Start.Month, y => y.Capacity)
                })
                .ToDictionary(pair => pair.resourceId, pair => pair.capacity);
            foreach (var resource in resources)
            {
                var capacity = new Dictionary<int, int>();
                if (capacities.ContainsKey(resource.Id))
                {
                    capacity = capacities[resource.Id];
                }

                viewModelList.Add(new ViewModelCapacity(
                    resource.Id,
                    resource.FirstName + ' ' + resource.LastName,
                    resource.ResourceGroupType.Type,
                    resource.ResourceGroupType.Group,
                    capacity));
            }

            var model = new Dictionary<string, List<ViewModelCapacity>>();
            foreach (var viewModel in viewModelList)
            {
                if (model.ContainsKey(viewModel.Group))
                {
                    model[viewModel.Group].Add(viewModel);
                }
                else
                {
                    model.Add(viewModel.Group, new List<ViewModelCapacity>() {viewModel});
                }
            }

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

            var resources = context.Set<Resource>()
                .Include(x => x.ResourceGroupType)
                .ToList();
            var resourceCapacities = context.Set<ResourceCapacity>();
            var periods = context.Set<Period>();

            var periodId = 0;
            var period = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year);
            if (period == null)
            {
                periods.Add(new Period(new DateTime(year, month, 1), new DateTime(year, month, 28)));
                context.SaveChanges();

                periodId = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year).Id;
                context.SaveChanges();
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

            context.SaveChanges();
        }
    }
}