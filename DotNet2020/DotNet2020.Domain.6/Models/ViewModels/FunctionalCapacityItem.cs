using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class FunctionalCapacityItem
    {
        public Period Period { get; set; }
        public int currentCapacity { get; set; }
        public int plannedCapacity { get; set; }
    }
}
