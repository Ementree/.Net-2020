using System;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;

namespace DotNet2020.Domain._6.Services
{
    public static class FunctionalCapacityService
    {
        static Tuple<int,int> GetYearRange(List<Period> periods)
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

        static List<FCPeriodWithBothCapacity> InitMounthsList(int year)
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

        static Dictionary<int,List<FCPeriodWithBothCapacity>> CreateDictWithDefoultList(Tuple<int,int> yearsRangeTuple)
        {
            var yearsDict = new Dictionary<int, List<FCPeriodWithBothCapacity>>();

            for (int i = yearsRangeTuple.Item1; i <= yearsRangeTuple.Item2; i++)
            {
                yearsDict[i] = InitMounthsList(i);
            }

            return yearsDict;
        }

        static bool GetFirstOrNullItem(List<FCPeriodWithBothCapacity> items,DateTime date)
        {
            foreach(var i in items)
            {
                if (i.Period.Start.Year == date.Year && i.Period.Start.Month == date.Month)
                    return true;
            }

            return false;
        }

        static void AddPeriodWithBothCapacityInDict(Dictionary<int,List<FCPeriodWithBothCapacity>> yearItemsDict,List<FCPeriodWithBothCapacity> items)
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

        static List<Period> GetAllPeriods(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = new List<Period>();

            foreach (var pair in viewModel.Dict)
            {
                foreach (var resource in pair.Value)
                {
                    foreach (var resourceItem in resource.Items)
                    {
                        allPeriods.Add(resourceItem.Period);
                    }
                }
            }

            return allPeriods;
        }

        public static void SortViewModelPeriodsInResourceOnYears(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = GetAllPeriods(viewModel);
            var yearsRangeTuple = GetYearRange(allPeriods);

            foreach(var pair in viewModel.Dict)
            {
                foreach(var resource in pair.Value)
                {
                    var dict = CreateDictWithDefoultList(yearsRangeTuple);

                    AddPeriodWithBothCapacityInDict(dict, resource.Items);

                    resource.YearItemsDict = dict;
                }
            }
        }

        public static void AddYearRange(FunctionalCapacityViewModel viewModel)
        {
            var allPeriods = GetAllPeriods(viewModel);

            var yearsRangeTuple = GetYearRange(allPeriods);
            viewModel.YearsRange = yearsRangeTuple;
        }
    }
}
