using System.Collections.Generic;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class Absences
{
    public List<Period> Periods { get; set; }

    public Dictionary<string, List<int>> ResourceAbsences { get; set; }
}
}
