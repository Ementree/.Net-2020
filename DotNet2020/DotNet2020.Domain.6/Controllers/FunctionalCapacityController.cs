using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DotNet2020.Domain._6.Services;

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
            //Предварительно стягивание с бд ResourceGroupType позволяет избежать .Include() в нескольких запросах
            var resourceType = _context.Set<ResourceGroupType>().ToList();

            var resourceWithCurrentPeriodCapacityDict = GetDataFromResourceCapacityTable();
            var resourcePeriodWithPlannedCapacityDict = GetDataFromFunctioningCapacityResourceTable();

            var tableLinePreformList = ComposeDataFromTables(resourceWithCurrentPeriodCapacityDict,resourcePeriodWithPlannedCapacityDict);
            var resourceWithBothPeriodCapacityDict = GetResourceWithBithCapcacityListDict(tableLinePreformList);

            var viewModel = GetFCViewModel(resourceWithBothPeriodCapacityDict);

            FunctionalCapacityService.SortViewModelPeriodsInResourceOnYears(viewModel);
            viewModel.YearsRange =  FunctionalCapacityService.GetViewModelYearRange(viewModel);
            viewModel.CurrentYear = FunctionalCapacityService.ValidateCurrentYear(viewModel, currentYear);
            viewModel.CurrentAccuracy = FunctionalCapacityService.ValidateAccuracy(currentAccuracy);
            

            return View(viewModel);
        }

        FunctionalCapacityViewModel GetFCViewModel(Dictionary<Resource, List<FCPeriodWithBothCapacity>> resourceWithBothPeriodCapacityDict)
        {
            var resourceWithTableDataListPreform = new List<FCResourceWithTableData>();
            var viewModel = new FunctionalCapacityViewModel();

            foreach (var resourceWithPeriodBothCapacityList in resourceWithBothPeriodCapacityDict)
            {
                resourceWithTableDataListPreform.Add(new FCResourceWithTableData()
                {
                    Resource = resourceWithPeriodBothCapacityList.Key,
                    PeiodWithBothCapacityList = resourceWithPeriodBothCapacityList.Value
                });
            }

            var GroupedResources = GetSortedOnGroupsResources(resourceWithTableDataListPreform);

            viewModel.GroupedResources = GroupedResources;

            return viewModel;
        }

        Dictionary<string,List<FCResourceWithTableData>> GetSortedOnGroupsResources(List<FCResourceWithTableData> resourceWithTableDataListPreform)
        {
            var groupedResources = new Dictionary<string, List<FCResourceWithTableData>>();

            foreach (var resourceWithTD in resourceWithTableDataListPreform)
            {
                var groupName = resourceWithTD.Resource.ResourceGroupType.Group;

                if (!groupedResources.ContainsKey(groupName))
                    groupedResources[groupName] = new List<FCResourceWithTableData>();

                groupedResources[groupName].Add(resourceWithTD);
            }

            return groupedResources;
        }

        Dictionary<Resource,List<FCPeriodWithBothCapacity>> GetResourceWithBithCapcacityListDict(List<FCTableLinePreform> tableLinePreformList)
        {
            return tableLinePreformList
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
        }

        Dictionary<Resource,List<CurrentPeriodCapacity>> GetDataFromResourceCapacityTable()
        {
            return _context.Set<ResourceCapacity>()
                .Include(rc => rc.Period)
                .Include(rc => rc.Resource)
                .ToList()
                .GroupBy(rc => rc.Resource)
                .ToDictionary(rc => rc.Key, rc => rc
                    .Select(g => new CurrentPeriodCapacity { Capacity = g.Capacity, Period = g.Period })
                        .ToList());
        }

        Dictionary<ResourcePeriodKey,int> GetDataFromFunctioningCapacityResourceTable()
        {
            return _context.Set<FunctioningCapacityResource>()
                .Include(fcr => fcr.Resource)
                .Include(fcr => fcr.Period)
                .ToList()
                .GroupBy(fcr => new ResourcePeriodKey { Resource = fcr.Resource, Period = fcr.Period })
                .ToDictionary(group => group.Key, group => group
                    .Select(fcr => fcr.FunctionCapacity)
                        .Sum());
        }

        List<FCTableLinePreform> ComposeDataFromTables(Dictionary<Resource, List<CurrentPeriodCapacity>> resourceWithCurrentPeriodCapacityDict,
            Dictionary<ResourcePeriodKey, int> resourcePeriodWithPlannedCapacityDict)
        {
            var tableLinePreformList = new List<FCTableLinePreform>();

            foreach (var resourceWithCurrentPeriodCapacityList in resourceWithCurrentPeriodCapacityDict)
            {
                var currentResource = resourceWithCurrentPeriodCapacityList.Key;

                foreach (var val in resourceWithCurrentPeriodCapacityList.Value)
                {
                    var newLineViewModel = new FCTableLinePreform()
                    {
                        Resource = currentResource,
                        Period = val.Period,
                        currentCapacity = val.Capacity,
                        plannedCapacity = 0
                    };

                    tableLinePreformList.Add(newLineViewModel);
                }
            }

            foreach (var p in resourcePeriodWithPlannedCapacityDict)
            {
                var currentResource = p.Key.Resource;
                var currentPeriod = p.Key.Period;
                var plannedCapacity = p.Value;

                foreach (var vm in tableLinePreformList)
                {
                    if (vm.Resource.Id == currentResource.Id && vm.Period.Id == currentPeriod.Id)
                    {
                        vm.plannedCapacity = plannedCapacity;
                        break;
                    }
                }
            }

            return tableLinePreformList;
        }
    }
}