using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;

namespace DotNet2020.Domain._6.Services
{
    public class CapacityWithAbsenceService
    {
        private static int monthInHours = 160;
        private static List<ResourceCapacity> _capacity;

        public CapacityWithAbsenceService(List<ResourceCapacity> capacity)
        {
            _capacity = capacity;
        }
        
        public List<ResourceCapacity> GetCapacityWithAbsence(List<AbstractCalendarEntry> absences)
        {
            foreach (var absence in absences)
            {
                var days = SplitTimespanToDays(absence.From, absence.To);

                foreach (var day in days)
                {
                    ModifyCapacity(day, absence.CalendarEmployeeId);
                }
            }

            return _capacity;
        }

        private static void ModifyCapacity(DateTime day, int resourceId)
        {
            if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
            {
                _capacity = _capacity
                    .Where(x => 
                        x.ResourceId == resourceId &&
                        x.Period.Start.Month == day.Month &&
                        x.Period.Start.Year == day.Year)
                    .Select(x =>
                    {
                        x.Capacity -= 5;
                        if (x.Capacity < 0) throw new Exception("capacity < 0");
                        return x;
                    })
                    .ToList();
            }
        }
        private static List<DateTime> SplitTimespanToDays(DateTime from, DateTime to)
        {
            var daysCount = (to - from).Days;
            var days = new List<DateTime>();
            var currentDay = from.Day;
            var currentMonth = from.Month;
            var currentYear = from.Year;

            for (int i = 0; i < daysCount; i++)
            {
                days.Add(new DateTime(currentYear, currentMonth, currentDay));
                if (currentDay + 1 > CountOfDaysByMonth(currentMonth, DateTime.IsLeapYear(currentYear)))
                {
                    currentDay = 1;
                    if (currentMonth == 12)
                    {
                        currentMonth = 1;
                        currentYear++;
                    }
                    else currentMonth++;
                }
                else currentDay++;
            }

            var last = days.Last();
            if (last.Day != to.Day || last.Month != to.Month || last.Year != to.Year) throw new Exception("Wrong split");
            return days;
        }
        private static int CountOfDaysByMonth(int month, bool leap)
        {
            switch (month)
            {
                case 1: return 31;
                case 2:
                    if (leap) return 29;
                    else return 28;
                case 3: return 31;
                case 4: return 30;
                case 5: return 31;
                case 6: return 30;
                case 7: return 31;
                case 8: return 31;
                case 9: return 30;
                case 10: return 31;
                case 11: return 30;
                case 12: return 31;
                default:
                    throw new Exception("Undefined month");
            }
        }
    }
}