using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class Resource
    {
        public Resource(int id, string firstName, string lastName, int resourceTypeId, ResourceType resourceType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            ResourceTypeId = resourceTypeId;
            ResourceType = resourceType;
        }
        protected Resource() : base() { }
        public int Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public int ResourceTypeId { get; protected set; }
        public virtual ResourceType ResourceType { get; protected set; }
    }
}
