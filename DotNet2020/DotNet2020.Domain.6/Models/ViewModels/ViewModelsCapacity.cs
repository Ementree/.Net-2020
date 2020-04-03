using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class ViewModelsCapacity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string,int> Capacity { get; set; }
        public ViewModelsCapacity(string name, string type, Dictionary<string, int> capacity)
        {
            Name = name;
            Type = type;
            Capacity = capacity;
        }
    }
}