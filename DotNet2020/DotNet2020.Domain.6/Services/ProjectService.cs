using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class ProjectService
    {
        private readonly DbContext _dbContext;

        public ProjectService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProjectViewModel> GetProjectsViewModels()
        {
            return _dbContext.Set<Project>()
                .Select(project =>
                    new ProjectViewModel(project.Id, project.Name, project.ProjectStatusId, null));
        }

        public ProjectViewModel GetProjectViewModelById(int id)
        {
            var a = _dbContext.Set<FunctioningCapacityResource>()
                .Where(resource => resource.ProjectId == id)
                .Include(resource => resource.Project)
                .ThenInclude(project => project.ProjectStatus)
                .Include(resource => resource.Resource)
                .ThenInclude(resource => resource.AppIdentityUser)
                .Include(resource => resource.Period)
                .OrderBy(resource => resource.Period.Start)
                .ToList()
                .GroupBy(resource => resource.Period)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());

            var project = _dbContext.Set<Project>().Find(id);
            var periodsCapacity = _dbContext.Set<FunctioningCapacityProject>()
                .Where(capacityProject => capacityProject.ProjectId == id)
                .Include(capacityProject => capacityProject.Period)
                .ToList();
            var periods2 = periodsCapacity.Select(periodCap =>
            {
                var fresArr = new ProjectViewModel.ResourceCapacityViewModel[0];
                if (a.ContainsKey(periodCap.Period))
                {
                    fresArr = a[periodCap.Period]
                        .Select(functioningCapacityResource =>
                            new ProjectViewModel.ResourceCapacityViewModel(functioningCapacityResource.Resource.Id,
                                $"{functioningCapacityResource.Resource.AppIdentityUser.FirstName} {functioningCapacityResource.Resource.AppIdentityUser.LastName}",
                                functioningCapacityResource.FunctionCapacity))
                        .ToArray();
                }
                var capacity = periodCap.FunctioningCapacity;
                var period = new ProjectViewModel.PeriodViewModel(capacity, periodCap.Period.Start, fresArr);
                return period;
            }).ToArray();
            var projectViewModel = new ProjectViewModel(id, project.Name, project.ProjectStatusId, periods2);
            return projectViewModel;
        }
    }
}