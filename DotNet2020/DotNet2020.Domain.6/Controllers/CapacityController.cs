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
                    key = x.Key,
                    value = x.ToDictionary(y => y.Period.Start.Month, y => y.Capacity)
                })
                .ToDictionary(pair => pair.key, pair => pair.value);
            foreach (var resource in resources)
            {
                var capacityDic = new Dictionary<int, int>();
                if (capacities.ContainsKey(resource.Id))
                {
                    capacityDic = capacities[resource.Id];
                }
                viewModelList.Add(new ViewModelCapacity(
                    resource.Id, 
                    resource.FirstName + ' ' + resource.LastName, 
                    resource.ResourceGroupType.Type, 
                    resource.ResourceGroupType.Group,
                    capacityDic));
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

            ViewBag.Months = GetMonths(year);
            ViewBag.MaxLengthName = GetMaxLengthName(model);
            return View(model);
        }

        private int GetMaxLengthName(Dictionary<string, List<ViewModelCapacity>> model)
        {
            var max = 0;
            foreach (var resources in model)
            {
                var tmax = resources.Value.Max(x => x.Name.Length);
                if (tmax > max) max = tmax;
            }

            return max;
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