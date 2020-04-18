using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DotNet2020.Domain._6.Services;
using System;

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
        public IActionResult Index(int currentYear = -1,int currentAccuracy = 5)       
        {
            //System.InvalidOperationException: 'Client side GroupBy is not supported.' ��� ToList()
            //� EFCore3.0 �� ��������
            var resourceType = _context.Set<ResourceGroupType>().ToList();

            var currentCapacityDict = _context.Set<ResourceCapacity>()
                .Include(rc => rc.Period)
                .Include(rc => rc.Resource)
                .ToList()
                .GroupBy(rc => rc.Resource)
                .ToDictionary(rc => rc.Key, rc => rc
                    .Select(g => new {capaity = g.Capacity, period = g.Period })
                        .ToList());

            var plannedCapacityDict = _context.Set<FunctioningCapacityResource>()
                .Include(fcr => fcr.Resource)
                .Include(fcr => fcr.Period)
                .ToList()
                .GroupBy(fcr => new { resource = fcr.Resource, period = fcr.Period })
                .ToDictionary(group => group.Key, group => group
                    .Select(fcr => fcr.FunctionCapacity)
                        .Sum());

            var viewModelLineList = new List<FCLine>();

            foreach(var p in currentCapacityDict)
            {
                var currentResource = p.Key;

                foreach(var val in p.Value)
                {
                    var newLineViewModel = new FCLine()
                    {
                        Resource = currentResource,
                        Period = val.period,
                        currentCapacity = val.capaity,
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
                       new FCPeriodWithBothCapacity
                       {
                           Period = vm.Period,
                           PlannedCapacity = vm.plannedCapacity,
                           CurrentCapacity = vm.currentCapacity
                       }).OrderBy(t => t.Period.Start)
                            .ToList());
            var itemsGroupPreform = new List<FCItemsGroup>();

            foreach(var itmesPair in itemsGroup)
            {


                itemsGroupPreform.Add(new FCItemsGroup()
                {
                    Resource = itmesPair.Key,
                    Items = itmesPair.Value
                });
            }

            var ViewModelDict = new Dictionary<string, List<FCItemsGroup>>();
            var viewModel = new FunctionalCapacityViewModel();
            var Periods = new List<Period>();

            foreach(var item in itemsGroupPreform)
            {
                var groupName = item.Resource.ResourceGroupType.Group;

                if (!ViewModelDict.ContainsKey(groupName))
                    ViewModelDict[groupName] = new List<FCItemsGroup>();

                var periods = item.Items.Select(i => i.Period).ToList();

                if (periods.Count > Periods.Count)
                    Periods = periods;

                ViewModelDict[groupName].Add(item);
            }

            if (currentYear == -1)
                viewModel.CurrentYear = DateTime.Now.Year;
            else
                viewModel.CurrentYear = currentYear;

            viewModel.Dict = ViewModelDict;
            viewModel.Periods = Periods;
            viewModel.CurrentAccuracy = currentAccuracy;

            if (currentYear == -1)
                viewModel.CurrentYear = DateTime.Now.Year;
            else
                viewModel.CurrentYear = currentYear;

            FunctionalCapacityService.SortViewModelPeriodsInResourceOnYears(viewModel);
            FunctionalCapacityService.AddYearRange(viewModel);

            return View(viewModel);
        }
    }
}