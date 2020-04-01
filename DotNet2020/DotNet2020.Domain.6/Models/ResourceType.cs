using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class ResourceType
    {
        public ResourceType(int id, string type)
        {
            Id = id;
            Type = type;
        }
        protected ResourceType() : base() { }
        public int Id { get; protected set; }
        public string Type { get; protected set; }
    }
}
