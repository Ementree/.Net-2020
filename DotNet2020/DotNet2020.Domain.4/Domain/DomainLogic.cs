using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Models;
using DotNet2020.Domain.Models.ModelView;
using Kendo.Mvc.Examples.Models.Scheduler;
using Microsoft.EntityFrameworkCore;

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
        
        public static List<CalendarEventViewModel> GetAllVacations(this DbContext context)
        {
            var allVacations = context.Set<AbstractCalendarEntry>()
                .Include(v => v.CalendarEmployee)
                .ToList()
                .Select(m =>
                    {
                        var color = "brown";
                        switch (m.AbsenceType)
                        {
                            case AbsenceType.Vacation:
                                if ((m as Vacation).IsApproved)
                                    color = "#59d27c";
                                else color = "#ff4242";
                                break;
                            case AbsenceType.SickDay:
                                color = "#95c8fd";
                                break;
                            case AbsenceType.Illness:
                                if ((m as Illness).IsApproved)
                                    color = "#6e84fe";
                                else color = "#ffff92";
                                break;
                        }
                        return new CalendarEventViewModel()
                        {
                            Id = m.Id,
                            Title = m.AbsenceType.ToString(),
                            Start = m.From,
                            End = m.To,
                            UserEmail = m.CalendarEmployee.UserName,
                            ColorId = color
                        };
                    }
                ).ToList();
            return allVacations;
        }
        
        public static List<UserViewModel> GetAllUsers(this DbContext context)
        {
            var users = context.Set<EmployeeCalendar>()
                .OrderBy(x => x.Employee.FirstName)
                .Select(u =>
                    new UserViewModel()
                    {
                        Name = u.UserName,
                        Email = u.UserName,
                        Color = "#6eb3fa"
                    })
                .ToList();
            return users;
        }
        
        public static List<string> GetAllHolidays(this DbContext context)
        {
            var holidays = context.Set<Holiday>()
                .ToList()
                .Select(u =>
                {
                    var year = u.Date.Year.ToString();
                    var month = u.Date.Month.ToString().StartsWith('0') ? u.Date.Month.ToString().Skip(1) : u.Date.Month.ToString();
                    var day = u.Date.Day.ToString().StartsWith('0') ? u.Date.Day.ToString().Skip(1) : u.Date.Day.ToString();
                    return $"{year}/{month}/{day}";
                })
                .ToList();
            return holidays;
        }
    }
}