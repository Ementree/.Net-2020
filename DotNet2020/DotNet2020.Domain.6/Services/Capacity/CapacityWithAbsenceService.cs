using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;

namespace DotNet2020.Domain._6.Services
{
    public class CapacityWithAbsenceService
    {
        private static List<ResourceCapacity> _capacity;
        private static Dictionary<int, int> _startCapacity;
        private static Dictionary<int, double> _tempCapacity;

        public CapacityWithAbsenceService(List<ResourceCapacity> capacity)
        {
            _capacity = capacity;
            _startCapacity = new Dictionary<int, int>();
            _tempCapacity = new Dictionary<int, double>();
            foreach (var cap in _capacity)
            {
                _startCapacity.Add(cap.Id, cap.Capacity);
                _tempCapacity.Add(cap.Id, cap.Capacity);
            }
        }
        
        public List<ResourceCapacity> GetCapacityWithAbsence(List<AbstractCalendarEntry> absences)
        {
            foreach (var absence in absences)
            {
                var days = SplitTimespanToDays(absence.From, absence.To);

                foreach (var day in days)
                {
                    CalculateCapacity(day, absence.CalendarEmployeeId);
                }
            }
            ModifyCapacity();
            return _capacity;
        }

        private static void ModifyCapacity()
        {
            foreach (var cap in _tempCapacity)

            {
                var res = _capacity.FirstOrDefault(c => c.Id == cap.Key);
                if (res != default)
                { 
                    res.Capacity = Convert.ToInt32(Math.Floor(cap.Value));  
                }
                
            }
        }
        private static void CalculateCapacity(DateTime day, int resourceId)
        {
            if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
            {
                var res = _capacity.FirstOrDefault(x =>
                    x.ResourceId == resourceId &&
                    x.Period.Start.Month == day.Month &&
                    x.Period.Start.Year == day.Year);
                if (res != default)
                {
                  double capacity = _startCapacity[res.Id];
                  _tempCapacity[res.Id] -= capacity / 20.0;           
                  if (_tempCapacity[res.Id] < 0) throw new Exception("capacity < 0");
                }
            }
        }
        private static List<DateTime> SplitTimespanToDays(DateTime from, DateTime to)
        {
            var daysCount = (to - from).Days;
            var days = new List<DateTime>();
            var currentDay = from.Day;
            var currentMonth = from.Month;
            var currentYear = from.Year;

            for (int i = 0; i < daysCount + 1; i++)
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