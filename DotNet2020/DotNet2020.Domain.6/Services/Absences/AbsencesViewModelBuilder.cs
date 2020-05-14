using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;

namespace DotNet2020.Domain._6.Services.Absences
{
    public class AbsencesViewModelBuilder
    {
        private readonly int _year;
        private readonly List<AbstractCalendarEntry> _absences;
        private readonly List<ResourceCapacity> _resourcesCapacity;
        private readonly List<FunctioningCapacityResource> _functioningCapacityResources;
        private readonly List<Resource> _resources;
        private readonly List<Period> _periods;

        public AbsencesViewModelBuilder(int year,IEnumerable<AbstractCalendarEntry> absences,IEnumerable<ResourceCapacity> resourcesCapacity,IEnumerable<FunctioningCapacityResource> functioningCapacityResources,IEnumerable<Resource> resources,IEnumerable<Period> periods)
        {
            _year = year;
            _absences = absences.ToList();
            _periods = periods.ToList();
            _resources = resources.ToList();
            _resourcesCapacity = resourcesCapacity.ToList();
            _functioningCapacityResources = functioningCapacityResources.ToList();
        }
        public Dictionary<string,AbsencesViewModel> Build()
        {
            var model = new Dictionary<string,AbsencesViewModel>();
            
            foreach (var resource in _resources.Where(resource => !model.ContainsKey(resource.ResourceGroupType.Group)))
            {
                model.Add(resource.ResourceGroupType.Group,MakeAbsencesViewModel(resource.ResourceGroupType.Group));
            }

            return model;
        }

        private string GetResourceName(Resource resource)
        {
            return $"{resource.Employee.FirstName} {resource.Employee.LastName}";
        }
        
        private Dictionary<string, (List<bool>, List<int>)> GetResourceAbsencesDictionary(string group)
        {
            var resourcesList = _resources.Where(res=>res.ResourceGroupType.Group == group).ToList();
            var resourceAbsences = resourcesList.ToDictionary(GetResourceName, res => (new List<bool>()
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
            }, new List<int>()
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            }));

            for (var i = 0; i < _periods.Count; i++)
            {
                var period = _periods[i];
                foreach (var resource in from resource in resourcesList let sumFunctioningCapacityResource = _functioningCapacityResources.Where(fc => fc.Period == period &&  GetResourceName(fc.Resource) == GetResourceName(resource)).Sum(fc => fc.FunctionCapacity) let resourceCapacity = _resourcesCapacity.FirstOrDefault(x => GetResourceName(x.Resource) == GetResourceName(resource)) where resourceCapacity != null && sumFunctioningCapacityResource > resourceCapacity.Capacity select resource)
                {
                    resourceAbsences[GetResourceName(resource)].Item1[i] = true;
                }
            }
            
            for (var i = 0; i < _periods.Count; i++)
            {
                var period = GetNormalizedPeriod(_periods[i]);
                foreach (var absence in _absences.Where(absence => absence.To >= period.Start && absence.From <= period.End))
                {
                    if (resourceAbsences.ContainsKey(absence.UserName))
                    {
                        resourceAbsences[absence.UserName].Item2[i] += (CalculateAbsences(period, absence));
                    }
                }
            }

            return resourceAbsences;
        }

        private AbsencesViewModel MakeAbsencesViewModel(string group)
        {
            return new AbsencesViewModel
            {
                Months = MonthGeneratorService.GetAllMonths(_year),
                ResourceAbsences = GetResourceAbsencesDictionary(group)
            };
        }
        
        private int CalculateAbsences(Period period,AbstractCalendarEntry entry)
        {
            var p = GetNormalizedPeriod(period);
            
            if (!(entry.To >= p.Start && entry.From <= p.End))
            {
                return 0;
            }

            if (entry.From >= p.Start && entry.To <= p.End)
            {
                return entry.To.Day - entry.From.Day + 1;
            }

            if (entry.From <= p.Start && entry.To >= p.End)
            {
                return p.End.Day - p.Start.Day + 1;
            }

            if (entry.From <= p.Start && entry.To <= p.End)
            {
                return entry.To.Day - p.Start.Day + 1;
            }

            if (entry.From >= p.Start && entry.To >= p.End)
            {
                return p.End.Day - entry.From.Day + 1;
            }
            return 0;
        }

        private Period GetNormalizedPeriod(Period period)
        {
            var endDay = DateTime.DaysInMonth(period.Start.Year, period.Start.Month);
            return new Period(period.Start,new DateTime(period.End.Year,period.End.Month,endDay));
        }
    }
}