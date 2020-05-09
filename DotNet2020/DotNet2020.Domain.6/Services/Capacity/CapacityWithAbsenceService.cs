using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;

namespace DotNet2020.Domain._6.Services
{
    public class CapacityWithAbsenceService
    {
        public static List<ResourceCapacity> GetCapacityWithAbsence(List<ResourceCapacity> capacity, List<AbstractCalendarEntry> absences)
        {
            /*foreach (var absence in absences)
            {
                var resource = capacity.FirstOrDefault(res => res.Id == absence.UserId)
            }*/
            return capacity;
        }
    }
}