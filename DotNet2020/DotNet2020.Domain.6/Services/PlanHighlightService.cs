using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;

namespace DotNet2020.Domain._6.Services
{
    public class PlanHighlightService
    {
        private readonly List<ResourceCapacity> _capacity;
        private readonly List<FunctioningCapacityProject> _funcCapacityProject;
        private readonly List<FunctioningCapacityResource> _funcCapacityResource;

        public PlanHighlightService(
            List<ResourceCapacity> capacity, 
            List<FunctioningCapacityProject> funcCapacityProject, 
            List<FunctioningCapacityResource> funcCapacityResource,
            List<AbstractCalendarEntry> absences)
        
        {
            _capacity = CapacityWithAbsenceService.GetCapacityWithAbsence(capacity, absences);
            _funcCapacityProject = funcCapacityProject;
            _funcCapacityResource = funcCapacityResource;
        }

        public List<FuncCapacityProjHighlight> GetFuncCapacityProjHighlight()
        {
            var funcCapacityProjHighlight = new List<FuncCapacityProjHighlight>();
            foreach (var fcp in _funcCapacityProject)
            {
                var sum = 0;
                foreach (var fcr in _funcCapacityResource)
                {
                    if (fcr.PeriodId == fcp.PeriodId && fcr.ProjectId == fcp.ProjectId)
                        sum += fcr.FunctionCapacity;
                }
                var color = "green-highlight";
                if (sum != fcp.FunctioningCapacity)
                    color = "red-highlight";
                funcCapacityProjHighlight.Add(new FuncCapacityProjHighlight() {PeriodId = fcp.PeriodId, ProjectId = fcp.ProjectId, Color = color});
            }
            
            return funcCapacityProjHighlight;
        }

        public List<FuncCapacityResourceHighlight> GetFuncCapacityResourceHighlight()
        {
            var funcCapacityResourceHighlight = new List<FuncCapacityResourceHighlight>();
            foreach (var fcr in _funcCapacityResource)
            {
                var resCapacity = _capacity.FirstOrDefault(cap => cap.PeriodId == fcr.PeriodId && cap.ResourceId == fcr.ResourceId);
                var sum = 0;
                var fcrInPeriod = _funcCapacityResource.Where(fcres => fcres.PeriodId == fcr.PeriodId);
                foreach (var value in fcrInPeriod)
                {
                    if (value.ResourceId == fcr.ResourceId)
                        sum += value.FunctionCapacity;
                }
                var color = "";
                if (resCapacity == default)
                    color = "red-highlight";
                else
                    color = sum > resCapacity.Capacity ? "red-highlight" : "";
                
                funcCapacityResourceHighlight.Add(new FuncCapacityResourceHighlight()
                {
                    Month = fcr.Period.Start.Month, 
                    Year = fcr.Period.Start.Year, 
                    ResourceId = fcr.ResourceId, 
                    ProjectId = fcr.ProjectId, 
                    Color = color
                });
            }
            
            return funcCapacityResourceHighlight;
        }
    }
}