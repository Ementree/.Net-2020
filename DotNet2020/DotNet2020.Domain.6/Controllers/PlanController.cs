using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models.ViewModels;
using DotNet2020.Domain._6.Services;
using Kendo.Mvc.Extensions;

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
                .Include(fres => fres.Resource)
                .ThenInclude(res => res.Employee)
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
                            group => group.ToList());
                    return KeyValuePair.Create(key, value);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            model.AddRange(GetProjectsWithoutResources(year));

            var newModel = model.GroupBy(pair => pair.Key.ProjectStatus.Status)
                .ToDictionary(pairs => pairs.Key,
                    pairs => pairs.ToDictionary(
                        pair => pair.Key, pair => pair.Value));

            var funcCapacitiesProject =
                _dbContext.Set<FunctioningCapacityProject>()
                    .Include(project => project.Period)
                    .Where(project => project.Period.Start.Year == year)
                    .ToList();
            var resourceCapacity = _dbContext.Set<ResourceCapacity>().ToList();

            ViewBag.FunctioningCapacityProject = funcCapacitiesProject;
            var highlightService =
                new PlanHighlightService(resourceCapacity, funcCapacitiesProject, functioningCapacityResources);
            ViewBag.FuncCapacityProjHighlight = highlightService.GetFuncCapacityProjHighlight();
            ViewBag.FuncCapacityResourceHighlight = highlightService.GetFuncCapacityResourceHighlight();
            ViewBag.CurrentDate = DateTime.Now;
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
                            resourceCapacityViewModel.Capacity = 0;
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

        [HttpPut]
        public bool EditProject([FromBody] ProjectViewModel projectViewModel)
        {
            //todo: сделать удаление челиков
            // using (var transaction = _dbContext.Database.BeginTransaction())
            // {
            //     var project = _dbContext.Set<Project>().Find(projectViewModel.Id);
            //     project.UpdateProjectInfo(projectViewModel.Name, projectViewModel.StatusId);
            //     _dbContext.Set<Project>().Update(project);
            //     _dbContext.SaveChanges();
            //
            //     foreach (var periodViewModel in projectViewModel.Periods
            //         .Where(model => model.Capacity <= 0 &&
            //                         model.Resources.Length == 0))
            //     {
            //         var functioningCapacityProjects = _dbContext
            //             .Set<FunctioningCapacityProject>();
            //         var projectCapacity = functioningCapacityProjects.FirstOrDefault(fcp =>
            //             fcp.Period.Start.Year == periodViewModel.Date.Year &&
            //             fcp.Period.Start.Month == periodViewModel.Date.Month &&
            //             fcp.ProjectId == projectViewModel.Id);
            //         if (periodViewModel.Resources.Length == 0 &&
            //             periodViewModel.Capacity == -1 &&
            //             projectCapacity != default)
            //         {
            //             functioningCapacityProjects.Remove(projectCapacity);
            //         }
            //     }
            //
            //     foreach (var periodViewModel in projectViewModel.Periods
            //         .Where(model => model.Capacity >= 0 ||
            //                         model.Resources.Length > 0))
            //     {
            //         //обновляем(создаем) мощность месяца в проекте
            //         var functioningCapacityProjects = _dbContext
            //             .Set<FunctioningCapacityProject>();
            //         //ищем нужный период в бд, если его нет то создаем
            //         var period = _dbContext.Set<Period>()
            //             .FirstOrDefault(p => p.Start.Year == periodViewModel.Date.Year &&
            //                                  p.Start.Month == periodViewModel.Date.Month);
            //         if (period == default)
            //         {
            //             var periodEntity = _dbContext.Set<Period>()
            //                 .Add(new Period(
            //                     new DateTime(periodViewModel.Date.Year, periodViewModel.Date.Month, 1),
            //                     new DateTime(periodViewModel.Date.Year, periodViewModel.Date.Month, 28)
            //                 ));
            //             _dbContext.SaveChanges();
            //             period = periodEntity.Entity;
            //         }
            //
            //         var projectCapacity = functioningCapacityProjects.FirstOrDefault(fcp =>
            //             fcp.Period.Start.Year == periodViewModel.Date.Year &&
            //             fcp.Period.Start.Month == periodViewModel.Date.Month &&
            //             fcp.ProjectId == projectViewModel.Id);
            //
            //         if (periodViewModel.Capacity == -1)
            //             periodViewModel.Capacity = 0;
            //         if (projectCapacity != default)
            //         {
            //             projectCapacity.UpdateFunctioningCapacity(periodViewModel.Capacity);
            //             functioningCapacityProjects.Update(projectCapacity);
            //         }
            //         else
            //         {
            //             var functioningCapacityProject = new FunctioningCapacityProject(projectViewModel.Id,
            //                 period.Id, periodViewModel.Capacity);
            //             functioningCapacityProjects.Add(functioningCapacityProject);
            //         }
            //
            //         _dbContext.SaveChanges();
            //         //тут людей обновляем
            //         var fResources = _dbContext.Set<FunctioningCapacityResource>();
            //
            //         foreach (var resource in fResources
            //             .Where(resource => resource.PeriodId == period.Id &&
            //                                resource.ProjectId == project.Id)
            //             .ToList())
            //         {
            //             if (!periodViewModel.Resources.Select(res => res.Id).Contains(resource.Id))
            //                 fResources.Remove(resource);
            //         }
            //
            //         _dbContext.SaveChanges();
            //         foreach (var resourceViewModel in periodViewModel.Resources)
            //         {
            //             if (resourceViewModel.Capacity == -1)
            //                 resourceViewModel.Capacity = 0;
            //             if (resourceViewModel.Id == -1)
            //                 continue;
            //             var functioningCapacityResource = fResources.FirstOrDefault(res =>
            //                 res.ResourceId == resourceViewModel.Id &&
            //                 res.ProjectId == projectViewModel.Id &&
            //                 res.PeriodId == period.Id);
            //             if (functioningCapacityResource != default)
            //             {
            //                 functioningCapacityResource.UpdateCapacity(resourceViewModel.Capacity);
            //                 fResources.Update(functioningCapacityResource);
            //             }
            //             else
            //             {
            //                 functioningCapacityResource = new FunctioningCapacityResource(projectViewModel.Id,
            //                     resourceViewModel.Id,
            //                     resourceViewModel.Capacity,
            //                     period.Id);
            //                 fResources.Add(functioningCapacityResource);
            //             }
            //
            //             _dbContext.SaveChanges();
            //         }
            //     }
            //
            //     transaction.Commit();
            //     return true;
            // }

            return EditProject1(projectViewModel);
        }

        [HttpGet("[controller]/getProjectPlanById/{id}")]
        public ProjectViewModel GetProjectPlanById(int id)
        {
            return _projectService.GetProjectViewModelById(id);
        }

        [HttpGet("[controller]/getProjectStatuses")]
        public IEnumerable<ProjectStatus> GetProjectStatuses()
        {
            var projectStatuses = new StatusesService(_dbContext).GetProjectStatuses();
            return projectStatuses;
        }

        [HttpGet("[controller]/getResources")]
        public IEnumerable<ProjectViewModel.ResourceCapacityViewModel> GetResources()
        {
            var projectStatuses = new ResourceService(_dbContext).GetResources()
                .Select(res => new ProjectViewModel.ResourceCapacityViewModel()
                {
                    Id = res.Id,
                    Name = $"{res.Employee.FirstName} {res.Employee.LastName}"
                });
            return projectStatuses;
        }

        private Dictionary<Project, Dictionary<Period, List<FunctioningCapacityResource>>>
            GetProjectsWithoutResources(int year)
        {
            var resources = _dbContext.Set<FunctioningCapacityResource>()
                .Select(res => res.ProjectId);
            var projects = _dbContext.Set<Project>()
                .Where(project => !resources.Contains(project.Id))
                .Include(project => project.ProjectStatus)
                .ToList();
            var periodDictionary = _dbContext.Set<Period>()
                .Where(period => period.Start.Date.Year == year)
                .ToDictionary(period => period,
                    period => new List<FunctioningCapacityResource>());
            return projects.ToDictionary(project => project, project => periodDictionary);
        }

        private bool EditProject1(ProjectViewModel projectViewModel)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var project = _dbContext.Set<Project>().Find(projectViewModel.Id);
                project.UpdateProjectInfo(projectViewModel.Name, projectViewModel.StatusId);
                _dbContext.Set<Project>().Update(project);
                _dbContext.SaveChanges();
                RemoveUnusedFuncCapacityProjects(projectViewModel.Periods, project.Id);
                foreach (var periodViewModel in projectViewModel.Periods)
                {
                    var period = GetPeriodForViewModel(periodViewModel);
                    RemoveUnusedFunctioningCapacityResources(periodViewModel.Resources, period.Id, project.Id);
                    
                    if (periodViewModel.Capacity < 0)
                    {
                        if (periodViewModel.Resources.Length > 0)
                        {
                            periodViewModel.Capacity = 0;
                            UpdateOrAddProjectPeriodCapacity(periodViewModel.Capacity, period.Id, project.Id);
                            foreach (var resource in periodViewModel.Resources)
                                AddFunctioningCapacityResource(resource, period.Id, project.Id);
                        }
                        else
                            RemoveFuncCapacityProject(period.Id, project.Id);
                    }
                    else
                    {
                        if (periodViewModel.Resources.Length > 0)
                        {
                            UpdateOrAddProjectPeriodCapacity(periodViewModel.Capacity, period.Id, project.Id);
                            foreach (var resource in periodViewModel.Resources)
                                AddFunctioningCapacityResource(resource, period.Id, project.Id);
                        }
                        else
                            UpdateOrAddProjectPeriodCapacity(periodViewModel.Capacity, period.Id, project.Id);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                return false;
            }
            transaction.Commit();
            transaction.Dispose();
            return true;
        }

        private Period GetPeriodForViewModel(ProjectViewModel.PeriodViewModel periodViewModel)
        {
            var period = _dbContext.Set<Period>()
                .FirstOrDefault(p => p.Start.Year == periodViewModel.Date.Year &&
                                     p.Start.Month == periodViewModel.Date.Month);
            if (period == default)
            {
                var periodEntity = _dbContext.Set<Period>()
                    .Add(new Period(
                        new DateTime(periodViewModel.Date.Year, periodViewModel.Date.Month, 1),
                        new DateTime(periodViewModel.Date.Year, periodViewModel.Date.Month, 28)
                    ));
                _dbContext.SaveChanges();
                period = periodEntity.Entity;
            }

            return period;
        }

        private void AddFunctioningCapacityResource(
            ProjectViewModel.ResourceCapacityViewModel resourceCapacityViewModel,
            int periodId,
            int projectId)
        {
            var capacityResources = _dbContext.Set<FunctioningCapacityResource>();
            var resource = capacityResources.FirstOrDefault(capacityResource =>
                capacityResource.PeriodId == periodId &&
                capacityResource.ResourceId == resourceCapacityViewModel.Id &&
                capacityResource.ProjectId == projectId);
            if (resource == default)
            {
                var newFResource = new FunctioningCapacityResource(projectId, resourceCapacityViewModel.Id,
                    resourceCapacityViewModel.Capacity, periodId);
                capacityResources.Add(newFResource);
            }
            else
            {
                resource.UpdateCapacity(resourceCapacityViewModel.Capacity);
                capacityResources.Update(resource);
            }

            _dbContext.SaveChanges();
        }

        private void RemoveUnusedFunctioningCapacityResources(
            ProjectViewModel.ResourceCapacityViewModel[] resourceCapacityViewModels,
            int periodId,
            int projectId)
        {
            var resourcesBefore = _dbContext
                .Set<FunctioningCapacityResource>()
                .Where(capacityResource => capacityResource.ProjectId == projectId &&
                                           capacityResource.PeriodId == periodId);
            foreach (var resourceBefore in resourcesBefore)
            {
                if (!resourceCapacityViewModels.Select(model => model.Id).Contains(resourceBefore.Id))
                    _dbContext
                        .Set<FunctioningCapacityResource>()
                        .Remove(resourceBefore);
            }

            _dbContext.SaveChanges();
        }

        private void UpdateOrAddProjectPeriodCapacity(int capacity, int periodId, int projectId)
        {
            var periodCapacities = _dbContext.Set<FunctioningCapacityProject>();
            var periodCapacity = periodCapacities
                .FirstOrDefault(functioningCapacityProject =>
                    functioningCapacityProject.ProjectId == projectId &&
                    functioningCapacityProject.PeriodId == periodId);
            if (periodCapacity == default)
            {
                var newPeriodCapacity = new FunctioningCapacityProject(projectId, periodId, capacity);
                periodCapacities.Add(newPeriodCapacity);
            }
            else
            {
                periodCapacity.UpdateFunctioningCapacity(capacity);
                periodCapacities.Update(periodCapacity);
            }

            _dbContext.SaveChanges();
        }

        private void RemoveFuncCapacityProject(int periodId, int projectId)
        {
            var periodCapacities = _dbContext.Set<FunctioningCapacityProject>();
            var periodCapacity = periodCapacities
                .FirstOrDefault(functioningCapacityProject =>
                    functioningCapacityProject.ProjectId == projectId &&
                    functioningCapacityProject.PeriodId == periodId);
            if (periodCapacity != default)
                periodCapacities.Remove(periodCapacity);
            _dbContext.SaveChanges();
        }

        private void RemoveUnusedFuncCapacityProjects(
            ProjectViewModel.PeriodViewModel[] periodViewModels,
            int projectId)
        {
            var funcPeriods = _dbContext.Set<FunctioningCapacityProject>();
            var projectPeriods = funcPeriods
                .Where(project => project.ProjectId == projectId)
                .Include(project => project.Period)
                .ToList();
            foreach (var projectPeriod in projectPeriods)
            {
                if (!periodViewModels
                    .Select(model => Tuple.Create(model.Date.Month, model.Date.Year))
                    .ToList()
                    .Contains(Tuple.Create(projectPeriod.Period.Start.Month, projectPeriod.Period.Start.Year)))
                    projectPeriods.Remove(projectPeriod);
            }

            _dbContext.SaveChanges();
        }
    }
}