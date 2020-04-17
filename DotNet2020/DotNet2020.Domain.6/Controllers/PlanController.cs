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
            //todo: капасити для проекта брать из бд
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
                        .ToDictionary(group => group.ToList().FirstOrDefault().Period, group => group.ToList());
                    return KeyValuePair.Create(key, value);
                })
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return View(model: model);
        }

        [HttpPut]
        public bool AddProject([FromBody] ProjectViewModel viewModel)
        {
            var project = new Project(viewModel.Name, 5);
            /*
            context.Set<Project>().Add(project);
            var periods = viewModel.Periods;
            var dbPeriods = context.Set<Period>();
            foreach (var period in periods)
            {
                var periodDb = context.Set<Period>()
                    .FirstOrDefault(p =>
                        (p.Start.Year == period.Date.Year && p.Start.Month == period.Date.Month));
                if (periodDb == default)
                {
                    var newPeriod = new Period(new DateTime(period.Date.Year, period.Date.Month, 1), 
                        new DateTime(period.Date.Year, period.Date.Month, 28));
                    var addRes = context.Set<Period>().Add(newPeriod);
                    var saveChanges = context.SaveChanges();
                    var id = addRes.Entity.Id;
                }

                foreach (var resourceCapacityViewModel in period.Resources)
                {
                    var fres = new FunctioningCapacityResource();
                }
                
            }
*/
            return true;
        }
    }
}