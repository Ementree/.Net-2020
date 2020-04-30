using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class FunctionalCapacityViewModel
    {
        public Tuple<int,int> YearsRange { get; set; }
        public Dictionary<string, List<FCResourceWithTableData>> GroupedResources { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentAccuracy { get; set; }
    }
}
