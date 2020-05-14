using System;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class AddCapacityService
    {
        private readonly DbContext _context;

        public AddCapacityService(DbContext context)
        {
            _context = context;
        }
        
        public int GetPeriodId(int month, int year)
        {
            var periods = _context.Set<Period>();
            var periodId = 0;
            var period = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year);
            if (period == default)
            {
                periods.Add(new Period(new DateTime(year, month, 1), new DateTime(year, month, 28)));
                _context.SaveChanges();

                periodId = periods.FirstOrDefault(per => per.Start.Month == month && per.Start.Year == year).Id;
            }
            else
            {
                periodId = period.Id;
            }

            return periodId;
        }

        public void AddResourceCapacity(int resourceId, int periodId, int capacity)
        {
            var resourceCapacities = _context.Set<ResourceCapacity>();
            var resourceCapacity =
                resourceCapacities.FirstOrDefault(res => res.ResourceId == resourceId && res.PeriodId == periodId);

            if (resourceCapacity == default)
            {
                resourceCapacities.Add(new ResourceCapacity(resourceId, capacity, periodId));
            }
            else
            {
                resourceCapacity.Capacity = capacity;
                resourceCapacities.Update(resourceCapacity);
            }
        }
    }
}