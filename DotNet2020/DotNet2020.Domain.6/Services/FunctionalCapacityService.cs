using System;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;

namespace DotNet2020.Domain._6.Services
{
    public class FunctionalCapacityService
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

        static List<FCItem> InitMounthsList(int year)
        {
            var result = new List<FCItem>();

            for(int i = 0; i < 12; i++)
            {
                result.Add(new FCItem()
                {
                    Period = new Period(new DateTime(year, i + 1, 1), new DateTime()),
                    CurrentCapacity = 0,
                    PlannedCapacity = 0
                });
            }

            return result;
        }

        static Dictionary<int,List<FCItem>> CreateDictWithDefoultList(Tuple<int,int> yearsRangeTuple)
        {
            var yearsDict = new Dictionary<int, List<FCItem>>();

            for (int i = yearsRangeTuple.Item1; i <= yearsRangeTuple.Item2; i++)
            {
                yearsDict[i] = InitMounthsList(i);
            }

            return yearsDict;
        }

        static bool GetFirstOrNullItem(List<FCItem> items,DateTime date)
        {
            foreach(var i in items)
            {
                if (i.Period.Start.Year == date.Year && i.Period.Start.Month == date.Month)
                    return true;
            }

            return false;
        }

        static void AddItemsInDict(Dictionary<int,List<FCItem>> yearItemsDict,List<FCItem> items)
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

        public static void SortViewModelOnYears(FunctionalCapacityViewModel viewModel)
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

            var yearsRangeTuple = GetYearRange(allPeriods);


            foreach(var pair in viewModel.Dict)
            {
                foreach(var resource in pair.Value)
                {
                    var dict = CreateDictWithDefoultList(yearsRangeTuple);

                    AddItemsInDict(dict, resource.Items);

                    resource.YearItemsDict = dict;
                }
            }
        }
    }
}
