using System;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class ProjectViewModel
    {
        public class ResourceCapacityViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Capacity { get; set; }
        }

        public class PeriodViewModel
        {
            public DateTime Date { get; set; }
            public ResourceCapacityViewModel[] Resources { get; set; }
        }

        public string Name { get; set; }
        public PeriodViewModel[] Periods { get; set; }
    }
}