using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class ResourceViewModel
    {
        public List<Resource> Resources { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Types { get; set; }
        private static DbContext _context;

        public ResourceViewModel(DbContext context)
        {
            _context = context;
            Resources = _context.Set<Resource>()
                .Include(r => r.Employee)
                .Include(r => r.ResourceGroupType)
                .ToList();
            SetGroupsAndTypes();
        }

        private void SetGroupsAndTypes()
        {
            var groups = new HashSet<string>();
            var types = new HashSet<string>();
            var groupsTypes = _context.Set<ResourceGroupType>().ToList();
            foreach (var groupType in groupsTypes)
            {
                if (!groups.Contains(groupType.Group))
                    groups.Add(groupType.Group);
                if (!types.Contains(groupType.Type))
                    types.Add(groupType.Type);
            }

            Groups = groups.ToList();
            Types = types.ToList();
        }
    }
}