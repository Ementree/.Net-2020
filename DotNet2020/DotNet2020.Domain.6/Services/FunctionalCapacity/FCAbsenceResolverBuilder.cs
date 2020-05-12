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

        public FCAbsenceResolverBuilder(DbContext context)
        {
            _context = context;
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

                if (fromDataTuple.Item1 == -1 && toDataTuple.Item1 != -1)
                {
                    var monthAbsenceWorkDaysDict =
                        GetWorkDaysCountWhenMRigth(new DateTime(year, toDataTuple.Item1, toDataTuple.Item2));
                    MergeDicts(result,monthAbsenceWorkDaysDict,name);
                }
                else if (fromDataTuple.Item1 != -1 && toDataTuple.Item1 == -1)
                {
                    
                }

            }

            return result;
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

        private Dictionary<string, int> GetWorkDaysCountWhenMRigth(DateTime mRigth)
        {
            var result = new Dictionary<string,int>();
            var monthNumber = mRigth.Month;
            var monthName = "";
            var lastDay = 0;

            for (int i = 1; i <= monthNumber;i++)
            {
                monthName = mRigth.GetMonthName().ToLower();

                if (i == monthNumber)
                    lastDay = monthNumber;
                else
                    lastDay = DateTime.DaysInMonth(mRigth.Year,i);

                var allWorkDays = GetWorkDaysCountInMonth(mRigth.Year, i, -1);
                var workDaysWithAbsence = GetWorkDaysCountInMonth(mRigth.Year, i, lastDay);
                
                int value = Convert.ToInt32((((double)(allWorkDays - workDaysWithAbsence))/(double) allWorkDays * 100));

                if (!result.ContainsKey(monthName))
                    result[monthName] = 0;

                result[monthName] += value;
            }

            return result;
        }

        private int GetWorkDaysCountInMonth(int year, int month,int lastDay)
        {
            var days= lastDay == -1? DateTime.DaysInMonth(year, month) : lastDay;
            int counter = 0;
            
            for (int i = 1; i <= days; i++)
            {
                var date = new DateTime(year,month,i);

                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Saturday)
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