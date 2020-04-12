using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain._6.Models.ViewModels;


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
            ViewBag.Months = GetMonths(year);
            return View(model);
        }

        private List<string> GetMonths(int year)
        {
            var months = new List<string>()
            {
                "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь",
                "Декабрь"
            };
            for (var i = 0; i < 12; i++)
            {
                months[i] += " " + year;
            }

            return months;
        }
    }
}