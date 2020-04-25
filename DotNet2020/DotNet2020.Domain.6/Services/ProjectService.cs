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
                .Where(resource => resource.ProjectId == id);

            var b = a.Include(resource => resource.Project)
                .ThenInclude(project => project.ProjectStatus)
                .Include(resource => resource.Resource)
                .Include(resource => resource.Period)
                .ToList();
            var c = b
                .GroupBy(resource => resource.Period);
            var d = c
                .ToDictionary(grouping => grouping.Key.Start, grouping => grouping.ToList());

            return null;
        }
    }
}