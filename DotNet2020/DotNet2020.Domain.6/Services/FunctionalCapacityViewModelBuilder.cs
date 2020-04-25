using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class FunctionalCapacityViewModelBuilderOptions
    {
        public int Year { get; set; }
        public int Accuracy { get; set; }

        public FunctionalCapacityViewModelBuilderOptions()
        {
            Year = -1;
            Accuracy = 5;
        }
    }

    public class FunctionalCapacityViewModelBuilder
    {
        readonly DbContext _context;

        public FunctionalCapacityViewModelBuilder(DbContext context)
        {
            _context = context;
        }

        public FunctionalCapacityViewModel Build(FunctionalCapacityViewModelBuilderOptions options)
        {
            var year = options.Year;
            var currentAccuracy = options.Accuracy;

            //Предварительно стягивание с бд ResourceGroupType позволяет избежать .Include() в нескольких запросах
            var resourceType = _context.Set<ResourceGroupType>().ToList();

            var resourceWithCurrentPeriodCapacityDict = GetDataFromResourceCapacityTable();
            var resourcePeriodWithPlannedCapacityDict = GetDataFromFunctioningCapacityResourceTable();

            var tableLinePreformList = ComposeDataFromTables(resourceWithCurrentPeriodCapacityDict, resourcePeriodWithPlannedCapacityDict);
            var resourceWithBothPeriodCapacityDict = GetResourceWithBithCapcacityListDict(tableLinePreformList);

            var viewModel = GetViewModel(resourceWithBothPeriodCapacityDict);

            SortViewModelPeriodsInResourceOnYears(viewModel);
            viewModel.YearsRange = GetViewModelYearRange(viewModel);
            viewModel.CurrentYear = ValidateCurrentYear(viewModel, year);
            viewModel.CurrentAccuracy = ValidateAccuracy(currentAccuracy);

            return viewModel;
        }

        Tuple<int,int> GetYearRange(List<Period> periods)
        {
            var minYear = int.MaxValue;
            var maxYear = 0;

            foreach(var p in periods)
            {
                if (p.Start.Year < minYear)
                    minYear = p.Start.Year;

                if (p.Start.Year > maxYear)
                    maxYear = p.Start.Year;
            }

            return Tuple.Create(minYear, maxYear);
        }

        List<FCPeriodWithBothCapacity> InitMonthsList(int year)
        {
            var result = new List<FCPeriodWithBothCapacity>();

            for(int i = 0; i < 12; i++)
            {
                result.Add(new FCPeriodWithBothCapacity()
                {
                    Period = new Period(new DateTime(year, i + 1, 1), new DateTime(year,i + 1,28)),
                    CurrentCapacity = 0,
                    PlannedCapacity = 0
                });
            }

            return result;
        }

        Dictionary<int,List<FCPeriodWithBothCapacity>> CreateDictWithDefaultList(Tuple<int,int> yearsRangeTuple)
        {
            var yearsDict = new Dictionary<int, List<FCPeriodWithBothCapacity>>();

            for (int i = yearsRangeTuple.Item1; i <= yearsRangeTuple.Item2; i++)
            {
                yearsDict[i] = InitMonthsList(i);
            }

            return yearsDict;
        }

        void AddPeriodWithBothCapacityInDict(Dictionary<int,List<FCPeriodWithBothCapacity>> yearItemsDict,List<FCPeriodWithBothCapacity> items)
        {
            foreach(var i in items)
            {
                var itemYear = i.Period.Start.Year;
                for(int j = 0; j < yearItemsDict[itemYear].Count; j++)
                {
                    if (yearItemsDict[itemYear][j].Period.Start.Month == i.Period.Start.Month)
                        yearItemsDict[itemYear][j] = i;
                }
            }
        }

        List<Period> GetAllPeriods(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = new List<Period>();

            foreach (var pair in viewModel.GroupedResources)
            {
                foreach (var resource in pair.Value)
                {
                    foreach (var resourceItem in resource.PeiodWithBothCapacityList)
                    {
                        allPeriods.Add(resourceItem.Period);
                    }
                }
            }

            return allPeriods;
        }

        void SortViewModelPeriodsInResourceOnYears(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = GetAllPeriods(viewModel);
            var yearsRangeTuple = GetYearRange(allPeriods);

            foreach(var pair in viewModel.GroupedResources)
            {
                foreach(var resource in pair.Value)
                {
                    var dict = CreateDictWithDefaultList(yearsRangeTuple);

                    AddPeriodWithBothCapacityInDict(dict, resource.PeiodWithBothCapacityList);

                    resource.YearItemsDict = dict;
                }
            }
        }

        Tuple<int,int> GetViewModelYearRange(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = GetAllPeriods(viewModel);

            var yearsRangeTuple = GetYearRange(allPeriods);

            return yearsRangeTuple;
        }

        int ValidateCurrentYear(FunctionalCapacityViewModel viewModel,int currentYear)
        {
            var range = GetViewModelYearRange(viewModel);
            var validatedYear = currentYear;

            if (currentYear < range.Item1 || currentYear > range.Item2)
                validatedYear = DateTime.Now.Year;

            return validatedYear;
        }

        int ValidateAccuracy(int currentAccuracy)
        {
            return currentAccuracy < 0 ? 0 : currentAccuracy;
        }

        FunctionalCapacityViewModel GetViewModel(Dictionary<Resource, List<FCPeriodWithBothCapacity>> resourceWithBothPeriodCapacityDict)
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

            var groupedResources = GetSortedOnGroupsResources(resourceWithTableDataListPreform);

            viewModel.GroupedResources = groupedResources;

            return viewModel;
        }

        Dictionary<string, List<FCResourceWithTableData>> GetSortedOnGroupsResources(List<FCResourceWithTableData> resourceWithTableDataListPreform)
        {
            var groupedResources = new Dictionary<string, List<FCResourceWithTableData>>();

            foreach (var resourceWithTd in resourceWithTableDataListPreform)
            {
                var groupName = resourceWithTd.Resource.ResourceGroupType.Group;

                if (!groupedResources.ContainsKey(groupName))
                    groupedResources[groupName] = new List<FCResourceWithTableData>();

                groupedResources[groupName].Add(resourceWithTd);
            }

            return groupedResources;
        }

        Dictionary<Resource, List<FCPeriodWithBothCapacity>> GetResourceWithBithCapcacityListDict(List<FCTableLinePreform> tableLinePreformList)
        {
            return tableLinePreformList
                .GroupBy(vm => vm.Resource)
                .ToDictionary(gr => gr.Key, gr => gr
                   .Select(vm =>
                       new FCPeriodWithBothCapacity
                       {
                           Period = vm.Period,
                           PlannedCapacity = vm.plannedCapacity,
                           CurrentCapacity = vm.currentCapacity
                       }).OrderBy(t => t.Period.Start)
                            .ToList());
        }

        Dictionary<Resource, List<CurrentPeriodCapacity>> GetDataFromResourceCapacityTable()
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

        Dictionary<ResourcePeriodKey, int> GetDataFromFunctioningCapacityResourceTable()
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
                int index;

                if(ContainsInPreformList(tableLinePreformList,p.Key,out index))
                {
                    tableLinePreformList[index].plannedCapacity = plannedCapacity;
                }
                else
                {
                    var lineViewModel = new FCTableLinePreform()
                    {
                        Resource = currentResource,
                        Period = currentPeriod,
                        currentCapacity = 0,
                        plannedCapacity = plannedCapacity
                    };

                    tableLinePreformList.Add(lineViewModel);
                }
            }

            return tableLinePreformList;
        }

        public bool ContainsInPreformList(List<FCTableLinePreform> preformList,ResourcePeriodKey resourcePeriodKey,out int index)
        {
            index = -1;
            for(int i = 0; i < preformList.Count(); i++) 
            { 
                if(preformList[i].Resource.Id == resourcePeriodKey.Resource.Id 
                    && preformList[i].Period.Id == resourcePeriodKey.Period.Id)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }
    }
}
