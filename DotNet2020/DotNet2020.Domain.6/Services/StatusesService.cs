using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class StatusesService
    {
        private readonly DbContext _dbContext;

        public StatusesService(DbContext context)
        {
            _dbContext = context;
        }

        public List<ProjectStatus> GetProjectStatuses()
        {
            return _dbContext.Set<ProjectStatus>().ToList();
        }
    }
}