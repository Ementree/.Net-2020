using System;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class ProjectViewModel
    {
        public class PeriodViewModel
        {
            public PeriodViewModel()
            {
            }

            public PeriodViewModel(int capacity, DateTime date, ResourceCapacityViewModel[] resources)
            {
                Capacity = capacity;
                Date = date;
                Resources = resources;
            }

            public int Capacity { get; set; }
            public DateTime Date { get; set; }
            public ResourceCapacityViewModel[] Resources { get; set; }
        }

        public class ResourceCapacityViewModel
        {
            public ResourceCapacityViewModel()
            {
            }

            public ResourceCapacityViewModel(int id, string name, int capacity)
            {
                Id = id;
                Name = name;
                Capacity = capacity;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public int Capacity { get; set; }
        }

        public ProjectViewModel()
        {
        }

        public ProjectViewModel(int id, string name, int statusId, PeriodViewModel[] periods)
        {
            Id = id;
            Name = name;
            StatusId = statusId;
            Periods = periods;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public PeriodViewModel[] Periods { get; set; }
    }
}