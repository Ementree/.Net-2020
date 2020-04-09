using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class FunctionalCapacityItem
    {
        public Period Period { get; set; }
        public int CurrentCapacity { get; set; }
        public int PlannedCapacity { get; set; }
    }

    public class FunctionalCapacityLine
    {
        public Resource Resource { get; set; }
        public Period Period { get; set; }
        public int currentCapacity { get; set; }
        public int plannedCapacity { get; set; }
    }

    public class FunctionalCapacityItemsGroup
    {
        public Resource Resource { get; set; }
        public List<FunctionalCapacityItem> Items { get; set; }
    }
}

