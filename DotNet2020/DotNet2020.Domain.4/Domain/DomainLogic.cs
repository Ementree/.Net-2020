using System;
using System.Collections.Generic;
using DotNet2020.Domain._4.Models;

namespace DotNet2020.Domain._4.Domain
{
    public static class DomainLogic
    {
        public static List<DateTime> GetDatesFromInterval(DateTime startDate, DateTime endDate)
        {
            List<DateTime> result = new List<DateTime>();
            if (startDate > endDate) return result;
            result.Add(startDate);
            while (startDate < endDate)
            {
                DateTime d = startDate.AddDays(1);
                result.Add(d);
                startDate = d;
            }
            return result;
        }
        
        
        public static int GetWorkDay(List<DateTime> days, List<Holiday> hollidays)
        {
            int total = 0;
            foreach (var day in days)
            {
                bool flag = true;
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    foreach (var holliday in hollidays)
                    {
                        if (day == holliday.Date)
                            flag = false;
                        continue;
                    }
                    if (flag)
                        total++;
                }
            }
            return total;
        }
    }
}