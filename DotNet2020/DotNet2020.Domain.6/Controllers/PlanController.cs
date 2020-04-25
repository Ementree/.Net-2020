using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models.ViewModels;
using DotNet2020.Domain._6.Services;

namespace DotNet2020.Domain._6.Controllers
{
    public class PlanController : Controller
    {
        private readonly DbContext _dbContext;
        private readonly ProjectService _projectService;
        public PlanController(DbContext dbDbContext)
        {
            _dbContext = dbDbContext;
            _projectService = new ProjectService(dbDbContext);
        }

        public IActionResult Index(int year = 0)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            ViewBag.Year = year;
            var functioningCapacityResources = _dbContext.Set<FunctioningCapacityResource>()
                .Include(fres => fres.Period)
                .Where(fres => fres.Period.Start.Year == year)
                .Include(fres => fres.Project)
                .ThenInclude(proj => proj.ProjectStatus)
                .Include(fres => fres.Resource)
                .ThenInclude(resource => resource.ResourceGroupType)
                .ToList();


            var groupBy = functioningCapacityResources.GroupBy(f => f.ProjectId);

            var dictionary = groupBy.ToDictionary(group =>
                    group.ToList().FirstOrDefault()?.Project,
                group => group.ToList());

            var model = dictionary.Select(pair =>
                {
                    var key = pair.Key;
                    var value = pair.Value.GroupBy(res => res.PeriodId)
                            .ToDictionary(group =>
                                    group.ToList().FirstOrDefault()?.Period,
                                group => @group.ToList())
                        ;

                    return KeyValuePair.Create(key, value);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var newModel = model.GroupBy(pair => pair.Key.ProjectStatus.Status)
                .ToDictionary(pairs => pairs.Key,
                    pairs => pairs.ToDictionary(
                        pair => pair.Key, pair => pair.Value));

            var funcCapacitiesProject =
                _dbContext.Set<FunctioningCapacityProject>()
                    .ToList();
            ViewBag.FunctioningCapacityProject = funcCapacitiesProject;
            return View(model: newModel);
        }

        [HttpPut]
        public bool AddProject([FromBody] ProjectViewModel viewModel)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var project = new Project(viewModel.Name, viewModel.StatusId);
                var projectEntity = _dbContext.Set<Project>().Add(project);
                _dbContext.SaveChanges();
                var projectId = projectEntity.Entity.Id;
                var periods = viewModel.Periods;
                foreach (var period in periods)
                {
                    var periodDb = _dbContext.Set<Period>()
                        .FirstOrDefault(p =>
                            (p.Start.Year == period.Date.Year && p.Start.Month == period.Date.Month));
                    int periodId;
                    if (periodDb == default)
                    {
                        var newPeriod = new Period(new DateTime(period.Date.Year, period.Date.Month, 1),
                            new DateTime(period.Date.Year, period.Date.Month, 28));
                        var addRes = _dbContext.Set<Period>().Add(newPeriod);
                        _dbContext.SaveChanges();
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
                        _dbContext.Set<FunctioningCapacityProject>().Add(functioningCapacityProject);
                    }
                    else
                    {
                        _dbContext.Set<FunctioningCapacityResource>().AddRange(functioningCapacityResources);
                        _dbContext.Set<FunctioningCapacityProject>().Add(functioningCapacityProject);
                    }

                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                transaction.Commit();
                return true;
            }
        }

        [HttpGet("getProjectPlanById/{id}")]
        public int GetProjectPlanById(int id)
        {
            _projectService.GetProjectViewModelById(id);
            return id;
        }
    }
}