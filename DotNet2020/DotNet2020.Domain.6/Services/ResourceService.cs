using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class ResourceService
    {
        private readonly DbContext _dbContext;

        public ResourceService(DbContext context)
        {
            _dbContext = context;
        }

        public List<Resource> GetResources()
        {
            return _dbContext.Set<Resource>()
                .Include(res => res.Employee)
                .OrderBy(resource => resource.Employee.LastName).ToList();
        }
    }
}