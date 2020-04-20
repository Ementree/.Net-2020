using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet2020.Domain._6.Models.ViewModels;
using Newtonsoft.Json;

namespace DotNet2020.Domain._6.Controllers
{
    public class PlanController : Controller
    {
        private readonly DbContext context;

        public PlanController(DbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index(int year = 2020)
        {
            ViewBag.Year = year;
            Dictionary<Project, Dictionary<Period, List<FunctioningCapacityResource>>> model;
            var functioningCapacityResources = context.Set<FunctioningCapacityResource>()
                .Include(fres => fres.Period)
                .Where(fres => fres.Period.Start.Year == year)
                .Include(fres => fres.Project)
                .ThenInclude(proj => proj.ProjectStatus)
                .Include(fres => fres.Resource)
                .ThenInclude(resource => resource.ResourceGroupType)
                .ToList();


            var groupBy = functioningCapacityResources.GroupBy(f => f.ProjectId);

            var dictionary = groupBy.ToDictionary(group =>
                    group.ToList().FirstOrDefault().Project,
                group => group.ToList());

            model = dictionary.Select(pair =>
                {
                    var key = pair.Key;
                    var value = pair.Value.GroupBy(res => res.PeriodId)
                            .ToDictionary(group =>
                                    group.ToList().FirstOrDefault().Period,
                                group => group.ToList())
                        ;

                    return KeyValuePair.Create(key, value);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var funcCapacitiesProject =
                context.Set<FunctioningCapacityProject>()
                    .ToList();
            ViewBag.FunctioningCapacityProject = funcCapacitiesProject;
            return View(model: model);
        }

        [HttpPut]
        public bool AddProject([FromBody] ProjectViewModel viewModel)
        {
            var project = new Project(viewModel.Name, 5);

            var projectEntity = context.Set<Project>().Add(project);
            context.SaveChanges();
            var projectId = projectEntity.Entity.Id;
            var periods = viewModel.Periods;
            foreach (var period in periods)
            {
                var periodDb = context.Set<Period>()
                    .FirstOrDefault(p =>
                        (p.Start.Year == period.Date.Year && p.Start.Month == period.Date.Month));
                int periodId;
                if (periodDb == default)
                {
                    var newPeriod = new Period(new DateTime(period.Date.Year, period.Date.Month, 1),
                        new DateTime(period.Date.Year, period.Date.Month, 28));
                    var addRes = context.Set<Period>().Add(newPeriod);
                    context.SaveChanges();
                    periodId = addRes.Entity.Id;
                }
                else
                {
                    periodId = periodDb.Id;
                }

                var projectPeriodCapacity = period.Capacity == -1 ? 0 : period.Capacity;
                var functioningCapacityProject =
                    new FunctioningCapacityProject(projectId, periodId, projectPeriodCapacity);
                var functioningCapacityResources = new List<FunctioningCapacityResource>();
                foreach (var resourceCapacityViewModel in period.Resources)
                {
                    if (resourceCapacityViewModel.Capacity == -1)
                        continue;
                    var fres = new FunctioningCapacityResource(projectId,
                        resourceCapacityViewModel.Id,
                        resourceCapacityViewModel.Capacity,
                        periodId);
                    functioningCapacityResources.Add(fres);
                }

                if (functioningCapacityResources.Count == 0 && functioningCapacityProject.FunctioningCapacity == 0)
                {
                    continue;
                }

                if (functioningCapacityResources.Count == 0 && functioningCapacityProject.FunctioningCapacity > 0)
                {
                    context.Set<FunctioningCapacityProject>().Add(functioningCapacityProject);
                }
                else
                {
                    context.Set<FunctioningCapacityResource>().AddRange(functioningCapacityResources);
                    context.Set<FunctioningCapacityProject>().Add(functioningCapacityProject);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}