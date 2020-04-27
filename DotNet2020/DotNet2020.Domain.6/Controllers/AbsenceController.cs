using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using DotNet2020.Domain._6.Services;
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
        public IActionResult Index(int year = 2020)
        {
            ViewBag.Year = year;
            var viewAbsences = new AbsencesViewModel();

            var absences = context.Set<CalendarEntry>().ToList();

            var resourcesCapacity = context.Set<ResourceCapacity>().ToList();
            var functioningCapacityResources = context.Set<FunctioningCapacityResource>()
                .Include(res => res.Resource)
                .ThenInclude(res => res.AppIdentityUser).ToList();
            var resources = context.Set<Resource>().Select(res => res.AppIdentityUser.FirstName + " " + res.AppIdentityUser.LastName).ToList();
            var periods = context.Set<Period>().Where(per => per.Start.Year == year).OrderBy(per => per.Start).ToList();
            viewAbsences.Months = MonthGeneratorService.GetAllMonths(year);
            var resourceAbsences = GetAbsencesVM(absences, resourcesCapacity, functioningCapacityResources, resources, periods);

            viewAbsences.ResourceAbsences = resourceAbsences;
            return View(viewAbsences);
        }

        private Dictionary<string, (List<bool>, List<int>)> GetAbsencesVM(List<CalendarEntry> absences, List<ResourceCapacity> resourcesCapacity, List<FunctioningCapacityResource> functioningCapacityResources, List<string> resources, List<Period> periods)
        {
            var resourceAbsences = new Dictionary<string, (List<bool>, List<int>)>();

            foreach (var res in resources)
            {
                resourceAbsences.Add(res, (new List<bool>() { false, false, false, false, false, false, false, false, false, false, false, false }, new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));
            }

            for (var i = 0; i < periods.Count; i++)
            {
                var period = periods[i];
                foreach (var resource in resources)
                {
                    var sumFunctioningCapacityResource = functioningCapacityResources.Where(fc => fc.Period == period && fc.Resource.AppIdentityUser.FirstName + " " + fc.Resource.AppIdentityUser.LastName == resource).Sum(fc => fc.FunctionCapacity);

                    var resourceCapacity = resourcesCapacity.FirstOrDefault(x => x.Resource.AppIdentityUser.FirstName + " " + x.Resource.AppIdentityUser.LastName == resource);

                    if (resourceCapacity != null && sumFunctioningCapacityResource > resourceCapacity.Capacity)
                    {
                        resourceAbsences[resource].Item1[i] = true;
                    }
                }
            }

            for (var i = 0; i < periods.Count; i++)
            {
                var period = periods[i];
                foreach (var absence in absences)
                {
                    if (absence.To >= period.Start && absence.From <= period.End)
                    {
                        resourceAbsences[absence.UserName].Item2[i] += (CalculateAbsences(period, absence));
                    }
                }
            }

            return resourceAbsences;
        }

        private int CalculateAbsences(Period period,CalendarEntry entry)
        {
            if (!(entry.To >= period.Start && entry.From <= period.End))
            {
                return 0;
            }

            if (entry.From >= period.Start && entry.To <= period.End)
            {
                return entry.To.Day - entry.From.Day;
            }

            if (entry.From <= period.Start && entry.To >= period.End)
            {
                return period.End.Day - period.Start.Day + 1;
            }

            if (entry.From <= period.Start && entry.To <= period.End)
            {
                return entry.To.Day - period.Start.Day;
            }

            if (entry.From >= period.Start && entry.To >= period.End)
            {
                return period.End.Day - entry.From.Day;
            }
            return 0;
        }
    }  
}