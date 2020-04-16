using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class FunctionalCapacityViewModel
    {
        public List<Period> Periods { get; set; }
        public Dictionary<string, List<FCItemsGroup>> Dict { get; set; }
    }
}
