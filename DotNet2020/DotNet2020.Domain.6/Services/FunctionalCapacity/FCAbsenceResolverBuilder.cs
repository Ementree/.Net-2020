using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                    //var monthAbsenceWorkDays = 
                }

            }

            return result;
        }

        private Dictionary<string, int> GetWorkDaysCountWhenMRigth(DateTime mRigth)
        {
            var result = new Dictionary<string,int>();
            var monthNumber = mRigth.Month;

            for (int i = 0; i < monthNumber;i++)
            {
                /*var mothName = 
                for (int i = 1; i <= mRigth.Day)
                {
                    
                }*/
            }

            return result;
        }

        private Tuple<int, int> GetSuitableMonthDay(DateTime date,int year)
        {
            var month = date.Year == year ? date.Month : -1;
            var day = date.Year == year ? date.Day : -1;
        }
    }
}