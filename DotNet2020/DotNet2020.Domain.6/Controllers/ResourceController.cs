using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Controllers
{
    public class ResourceController : Controller
    {
        private readonly DbContext _context;

        public ResourceController(DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var resourceViewModel = new ResourceViewModel(_context);
            return View(resourceViewModel);
        }

        [HttpPost]
        public IActionResult Index(List<string> groups, List<string> types)
        {
            var resources = _context.Set<Resource>()
                .Include(r => r.Employee)
                .Include(r => r.ResourceGroupType);
            var groupsAndTypes = _context.Set<ResourceGroupType>().ToList();

            for (int i = 0; i < groups.Count; i++)
            {
                var resourceId = int.Parse(groups[i].Split(';')[0]);
                var group = groups[i].Split(';')[1];
                if (resourceId != int.Parse(types[i].Split(';')[0])) throw new Exception("Resources didn't match");
                var type = types[1].Split(';')[1];

                var resource = resources.FirstOrDefault(res => res.Id == resourceId);
                if (resource == default) throw new Exception("Resource not found");


                var changed = false;
                if (resource.ResourceGroupType.Group != group || resource.ResourceGroupType.Type != type)
                {
                    var resourceGroupType = groupsAndTypes.FirstOrDefault(gt => gt.Group == group && gt.Type == type);
                    if (resourceGroupType == default) throw new Exception("Pair doesn't exist");
                    var id = resourceGroupType.Id;
                    resource.UpdateGroupAndType(id);
                    changed = true;
                }

                if (changed)
                    _context.SaveChanges();
            }

            var model = new ResourceViewModel(_context);
            return View(model);
        }
    }
}