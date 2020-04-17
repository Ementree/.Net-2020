using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class FCPeriodWithBothCapacity
    {
        public Period Period { get; set; }
        public int CurrentCapacity { get; set; }
        public int PlannedCapacity { get; set; }
    }

    public class FCLine
    {
        public Resource Resource { get; set; }
        public Period Period { get; set; }
        public int currentCapacity { get; set; }
        public int plannedCapacity { get; set; }
    }

    public class FCItemsGroup
    {
        public Resource Resource { get; set; }
        public List<FCPeriodWithBothCapacity> Items { get; set; }
        public Dictionary<int,List<FCPeriodWithBothCapacity>> YearItemsDict { get; set; }
    }
}

