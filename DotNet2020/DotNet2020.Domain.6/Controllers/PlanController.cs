using DotNet2020.Domain._6.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._6.Controllers
{
    public class PlanController : Controller
    {
        private static List<FunctioningCapacityResource> functioningCapacityResources;
        public PlanController()
        {
            var resType = new ResourceGroupType(1, "1", "worker");
            var resource1 = new Resource(1, "First", "1", resType.Id, resType);
            var resource2 = new Resource(2, "Second", "2", resType.Id, resType);
            var resource3 = new Resource(3, "Third", "3", resType.Id, resType);
            var resource4 = new Resource(4, "Last", "4", resType.Id, resType);

            var projStatus = new ProjectStatus(1, "In work");
            var proj1 = new Project(1, "Plans", projStatus.Id, projStatus);
            var proj2 = new Project(2, "Net DB", projStatus.Id, projStatus);
            var projects = new List<Project>() { proj1, proj2 };
            var periods = new List<Period>();
            for (int i = 0; i < 12; i++)
            {
                if (i != 11)
                    periods.Add(new Period(i+1, new DateTime(2020, i + 1, 20), new DateTime(2020, i + 2, 20)));
                else
                    periods.Add(new Period(i+1, new DateTime(2020, i + 1, 20), new DateTime(2021, 1, 20)));
            }
            int k = 1;
            functioningCapacityResources = new List<FunctioningCapacityResource>();
            for (int i = 0; i < 12; i++)
            {
                functioningCapacityResources
                    .Add(new FunctioningCapacityResource(k++, proj1.Id, resource1.Id, 20, periods[i].Id, proj1, resource1, periods[i]));
                functioningCapacityResources
                    .Add(new FunctioningCapacityResource(k++, proj1.Id, resource3.Id, 20, periods[i].Id, proj1, resource3, periods[i]));


                functioningCapacityResources
                    .Add(new FunctioningCapacityResource(k++, proj2.Id, resource2.Id, 20, periods[i].Id, proj2, resource1, periods[i]));
                functioningCapacityResources
                    .Add(new FunctioningCapacityResource(k++, proj2.Id, resource4.Id, 20, periods[i].Id, proj2, resource3, periods[i]));
            }
        }
        public IActionResult Index()
        {
            var model = functioningCapacityResources
                .GroupBy(f => f.ProjectId)
                .ToDictionary(group=> group.ToList().FirstOrDefault().Project, group=>group.ToList())
                .Select(pair =>
                {
                    var key = pair.Key;
                    var value = pair.Value.GroupBy(res => res.PeriodId)
                    .ToDictionary(group => group.ToList().FirstOrDefault().Period, group => group.ToList());
                    return KeyValuePair.Create(key, value);
                })
                .ToDictionary(pair=>pair.Key, pair=>pair.Value)
                ;
            return View(model: model);
        }
    }
}
