using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class CapacityViewModel
    {
        public Dictionary<string, List<CapacityViewModelData>> Data { get; set; }
        public List<string> Months { get; set; }
        public bool WithAbsence { get; set; }

        public CapacityViewModel(Dictionary<string, List<CapacityViewModelData>> data, List<string> months, bool withAbsence)
        {
            Data = data;
            Months = months;
            WithAbsence = withAbsence;
        }
    }
}