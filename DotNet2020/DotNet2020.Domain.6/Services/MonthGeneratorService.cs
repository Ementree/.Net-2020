using System;
using System.Collections.Generic;
using DotNet2020.Domain._6.ExtensionMethods;

namespace DotNet2020.Domain._6.Services
{
    public class MonthGeneratorService
    {
        public static List<string> GetAllMonths(int year)
        {
            var months = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                var date = new DateTime(year, i + 1, 1);
                months.Add($"{date.GetMonthName()} {year}");
            }

            return months;
        }

        public static List<string> GetMonthNames()
        {
            var months = new List<string>();
            for (var i = 0; i < 12; i++)
            {
                var date = new DateTime(2000, i + 1, 1);
                months.Add(date.GetMonthName());
            }

            return months;
        }
    }
}