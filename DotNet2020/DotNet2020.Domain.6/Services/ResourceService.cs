using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._6.Services
{
    public class ResourceService
    {
        private readonly DbContext dbContext;

        public ResourceService(DbContext context)
        {
            dbContext = context;
        }
        /*public class ResourceToSelect
        {
            public int Id { get; protected set; }
            public string FullName{ get; protected set; }

            public ResourceToSelect(int id, string fullName)
            {
                Id = id;
                FullName = fullName;
            }
        }*/

        public List<Resource> GetResources()
        {
            return dbContext.Set<Resource>().OrderBy(resource => resource.LastName).ToList();
        }
    }
}