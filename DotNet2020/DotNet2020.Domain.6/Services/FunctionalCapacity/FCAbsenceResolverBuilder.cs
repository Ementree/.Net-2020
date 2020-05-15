using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.ExtensionMethods;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotNet2020.Domain._6.Services
{
    public class FCAbsenceResolverBuilder
    {
        private readonly DbContext _context;
        private readonly List<Period> Periods;
        private readonly List<ResourceCapacity> Capacities;

        public FCAbsenceResolverBuilder(DbContext context)
        {
            _context = context;
            Periods = _context.Set<Period>().ToList();
            Capacities = _context.Set<ResourceCapacity>().ToList();
        }

        public Dictionary<string, Dictionary<string, int>> Build(int year)
        {
            var entryList = GetSuitableData(year);
            var result = GetAbsenceDict(entryList, year);

            return result;
        }

        private List<AbstractCalendarEntry> GetSuitableData(int year)
        {
            return _context.Set<AbstractCalendarEntry>()
                .Include(e => e.CalendarEmployee)
                .ThenInclude(ce => ce.Employee)
                .ToList()
                .Where(e => e.From.Year == year || e.To.Year == year)
                .ToList();
        }
        
        private Dictionary<string,Dictionary<string,int>> GetAbsenceDict(List<AbstractCalendarEntry> calendarEntries,int year)
        {
            var result = new Dictionary<string,Dictionary<string,int>>();

            foreach (var e in calendarEntries)
            {
                var name = e.CalendarEmployee.Employee.FirstName + " " + e.CalendarEmployee.Employee.LastName;
                if (!result.ContainsKey(name))
                {
                    result[name] = new Dictionary<string, int>();
                }
                
                var fromDataTuple = GetSuitableMonthDay(e.From, year);
                var toDataTuple = GetSuitableMonthDay(e.To, year);
                
                var monthAbsenceWorkDaysDict =
                    GetWorkDaysCountFromDataRange(fromDataTuple,toDataTuple,year,e);
                MergeDicts(result,monthAbsenceWorkDaysDict,name);
                
            }

            return result;
        }

        private int GetCoefficient(AbstractCalendarEntry e,int year,int month)
        {
            var periodId = Periods
                .First(p => p.Start.Year == year && p.Start.Month == month)
                .Id;
            
            var resourceId = e.CalendarEmployee.Employee.Id;

            return Capacities
                .First(rc => rc.ResourceId == resourceId && rc.PeriodId == periodId)
                .Capacity;

        }

        private void MergeDicts(Dictionary<string, Dictionary<string, int>> result, Dictionary<string, int> source,
            string key)
        {
            foreach (var p in source)
            {
                if (!result[key].ContainsKey(p.Key))
                    result[key][p.Key] = p.Value;
                else
                    result[key][p.Key] += p.Value;
            }
        }

        private Dictionary<string, int> GetWorkDaysCountFromDataRange(Tuple<int,int> fromDataTuple,Tuple<int,int> toDataTuple,int year,AbstractCalendarEntry e)
        {
            var startDay = 0;
            var endDay = 0;
            var startMonth = 0;
            var endMonth = 0;
            var result = new Dictionary<string,int>();

            if (fromDataTuple.Item1 == -1 && toDataTuple.Item1 != -1)
            {
                startMonth = 1;
                startDay = 1;
                endMonth = toDataTuple.Item1;
                endDay = toDataTuple.Item2;
            }
            else if(fromDataTuple.Item1 != -1 && toDataTuple.Item1 == -1)
            {
                startMonth = fromDataTuple.Item1;
                startDay = fromDataTuple.Item2;
                endMonth = 12;
                endDay = 31;
            }
            else
            {
                startMonth = fromDataTuple.Item1;
                startDay = fromDataTuple.Item2;
                endMonth = toDataTuple.Item1;
                endDay = toDataTuple.Item2;
            }
            

            for (int i = startMonth; i <= endMonth;i++)
            {
                string monthName = new DateTime(year,i,1).GetMonthName().ToLower();
                var coef = GetCoefficient(e,year, i);

                var lastDay = 0;
                if (i == endMonth)
                    lastDay = endDay;
                else
                    lastDay = DateTime.DaysInMonth(year,i);

                var firstDay = 0;
                if (i != startMonth)
                    firstDay = 1;
                else
                {
                    firstDay = startDay;
                }
                
                
                var allWorkDays = GetWorkDaysCountInMonth(year, i, 1,DateTime.DaysInMonth(year,i));
                var workDaysWithAbsence = GetWorkDaysCountInMonth(year, i, firstDay,lastDay);

                int value = Convert.ToInt32((((double)(workDaysWithAbsence))/(double) allWorkDays * coef));

                if (!result.ContainsKey(monthName))
                    result[monthName] = 0;

                result[monthName] += value;
            }

            return result;
        }

        private int GetWorkDaysCountInMonth(int year, int month,int monthStartDay,int monthEndDay)
        {
            int counter = 0;
            
            for (int i = monthStartDay; i <= monthEndDay; i++)
            {
                var date = new DateTime(year,month,i);

                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    counter++;
            }

            return counter;
        }

        private Tuple<int, int> GetSuitableMonthDay(DateTime date,int year)
        {
            var month = date.Year == year ? date.Month : -1;
            var day = date.Year == year ? date.Day : -1;

            return Tuple.Create(month, day);
        }
    }
}