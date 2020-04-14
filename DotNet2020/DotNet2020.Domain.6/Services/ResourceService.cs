using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._6.Models;

namespace DotNet2020.Domain._6.Services
{
    public class ResourceService
    {
        public class ResourceToSelect
        {
            public int Id { get; protected set; }
            public string FullName{ get; protected set; }

            public ResourceToSelect(int id, string fullName)
            {
                Id = id;
                FullName = fullName;
            }
        }

        public static List<ResourceToSelect> GetResources()
        {
            return Enumerable.Range(1, 7)
                .Select(item => new ResourceToSelect(item, $"Name LastName {item}"))
                .ToList();
        }
    }
}