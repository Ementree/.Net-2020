using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._6.Controllers
{
    public class AbsenceController : Controller
    {
        private DbContext context;
        public AbsenceController(DbContext context)
        {
            this.context = context;
        }
        // GET
        public IActionResult Index()
        {
            int year = 2020;
            ViewBag.Year = year;
            var viewAbsences = new Absences();

            var absences = new List<CalendarEntry>();
            absences.Add(new CalendarEntry() { UserName = "User ",From = new System.DateTime(2020,06,08),To = new System.DateTime(2020,6,15)});
            absences.Add(new CalendarEntry() { UserName = "User 2", From = new System.DateTime(2020, 05, 02), To = new System.DateTime(2020, 5, 15) });
            absences.Add(new CalendarEntry() { UserName = "User 3", From = new System.DateTime(2020, 06, 05), To = new System.DateTime(2020, 7, 15) });
            absences.Add(new CalendarEntry() { UserName = "User ", From = new System.DateTime(2020, 06, 05), To = new System.DateTime(2020, 7, 15) });
            absences.Add(new CalendarEntry() { UserName = "User ", From = new System.DateTime(2020, 04, 06), To = new System.DateTime(2020, 6, 15) });



            var periods = context.Set<Period>().ToList();
            
            viewAbsences.Periods = periods;

            var resourceAbsences = new Dictionary<string, List<int>>();

            var periodsList = new List<int>();
            foreach(var per in periods)
            {
                periodsList.Add(0);
            }

            for (var i = 0; i < periods.Count;i++)
            {
                var period = periods[i];
                foreach (var absence in absences)
                {
                    if (!resourceAbsences.ContainsKey(absence.UserName))
                    {
                        resourceAbsences.Add(absence.UserName, new List<int>(periodsList));
                    }
                    if (!Contains(period, absence))
                    {
                        continue;
                    }
                    else
                    {
                        resourceAbsences[absence.UserName][i] += (CalculateAbsences(period, absence));
                    }
                }
            }

            viewAbsences.ResourceAbsences = resourceAbsences;
            return View(viewAbsences);
        }

        private int CalculateAbsences(Period period,CalendarEntry entry)
        {
            if (!Contains(period, entry))
            {
                return 0;
            }

            if (entry.From > period.Start && entry.To < period.End)
            {
                return entry.To.Day - entry.From.Day;
            }
            else
            {
                if (entry.From < period.Start && entry.To > period.End)
                {
                    return period.End.Day - period.Start.Day;
                }
                else
                {
                    if (entry.From < period.Start && entry.To < period.End)
                    {
                        return entry.To.Day - period.Start.Day;
                    }
                    else
                    {
                        if (entry.From > period.Start && entry.To > period.End)
                        {
                            return period.End.Day - entry.From.Day;
                        }
                    }
                }
            }
            return 0;
        }

        private bool Contains(Period period,CalendarEntry entry)
        {
            return entry.To > period.Start && entry.From < period.End;
        }
    }

    
}