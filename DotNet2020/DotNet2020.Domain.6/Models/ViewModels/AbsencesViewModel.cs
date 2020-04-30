using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class AbsencesViewModel
    {
        public List<string> Months { get; set; }

        public Dictionary<string,(List<bool>, List<int>)> ResourceAbsences { get; set; }
    }
}
