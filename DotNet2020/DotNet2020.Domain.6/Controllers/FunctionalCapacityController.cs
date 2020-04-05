using Microsoft.AspNetCore.Mvc;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models.ViewModels;
using DotNet2020.Domain._6.ExtensionMethods;

namespace DotNet2020.Domain._6.Controllers
{
    public class FunctionalCapacityController : Controller
    {
        public readonly DbContext _context;

        public FunctionalCapacityController(DbContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index()
        {
            //System.InvalidOperationException: 'Client side GroupBy is not supported.' без ToList()
            //в EFCore3.0 не работает
            var resourceType = _context.Set<ResourceGroupType>().ToList();

            var currentCapacityDict = _context.Set<ResourceCapacity>()
                .Include(rc => rc.Period)
                .Include(rc => rc.Resource)
                .ToList()
                .GroupBy(rc => rc.Resource)
                .ToDictionary(rc => rc.Key, rc => rc
                    .Select(g => new {capatiy = g.Capacity, period = g.Period })
                        .ToList());

            var plannedCapacityDict = _context.Set<FunctioningCapacityResource>()
                .Include(fcr => fcr.Resource)
                .Include(fcr => fcr.Period)
                .ToList()
                .GroupBy(fcr => new { resource = fcr.Resource, period = fcr.Period })
                .ToDictionary(group => group.Key, group => group
                    .Select(fcr => fcr.FunctionCapacity)
                        .Sum());

            var viewModelLineList = new List<FunctionalCapacityLine>();

            foreach(var p in currentCapacityDict)
            {
                var currentResource = p.Key;

                foreach(var val in p.Value)
                {
                    var newLineViewModel = new FunctionalCapacityLine()
                    {
                        Resource = currentResource,
                        Period = val.period,
                        currentCapacity = val.capatiy,
                        plannedCapacity = 0
                    };

                    viewModelLineList.Add(newLineViewModel);
                }
            }

            foreach(var p in plannedCapacityDict)
            {
                var currentResource = p.Key.resource;
                var currentPeriod = p.Key.period;
                var plannedCapacity = p.Value;

                foreach(var vm in viewModelLineList)
                {
                    if (vm.Resource.Id == currentResource.Id && vm.Period.Id == currentPeriod.Id)
                    { 
                        vm.plannedCapacity = plannedCapacity;
                        break;
                    }
                }
            }

            var itemsGroup = viewModelLineList
                .GroupBy(vm => vm.Resource)
                .ToDictionary(vm => vm.Key, vm => vm
                   .Select(vm =>
                       new FunctionalCapacityItem
                       {
                           Period = vm.Period,
                           PlannedCapacity = vm.plannedCapacity,
                           CurrentCapacity = vm.currentCapacity
                       }).OrderBy(t => t.Period.Start)
                            .ToList());
            var itemsGroupPreform = new List<FunctionalCapacityItemsGroup>();

            foreach(var itmesPair in itemsGroup)
            {
                itemsGroupPreform.Add(new FunctionalCapacityItemsGroup()
                {
                    Resource = itmesPair.Key,
                    Items = itmesPair.Value
                });
            }

            var ViewModelDict = new Dictionary<string, List<FunctionalCapacityItemsGroup>>(); ;

            foreach(var item in itemsGroupPreform)
            {
                var groupName = item.Resource.ResourceGroupType.Group;

                if (!ViewModelDict.ContainsKey(groupName))
                    ViewModelDict[groupName] = new List<FunctionalCapacityItemsGroup>();

                ViewModelDict[groupName].Add(item);
            }

            return View(ViewModelDict);
        }
    }
}