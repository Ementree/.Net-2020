using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class ViewModelCapacity
    {
        public int ResourceId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public Dictionary<int,int> Capacity { get; set; }
        public ViewModelCapacity(int resourceId, string name, string type, string group, Dictionary<int, int> capacity)
        {
            ResourceId = resourceId;
            Name = name;
            Type = type;
            Group = group;
            Capacity = capacity;
        }
        
    }
}