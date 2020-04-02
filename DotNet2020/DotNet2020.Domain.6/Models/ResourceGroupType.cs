using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class ResourceGroupType
    {
        public ResourceGroupType(int id, string type,string group)
        {
            Id = id;
            Type = type;
            Group = group;
        }
        protected ResourceGroupType() : base() { }
        public int Id { get; protected set; }
        public string Type { get; protected set; }
        public string Group { get; protected set; }
    }
}
