using DotNet2020.Domain._5.Entities;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Models
{
    public class ChartModel
    {
        public List<Chart> Charts { get; private set; }
        public int ReportId { get; private set; }

        public ChartModel(int reportId, List<Chart> charts)
        {
            ReportId = reportId;
            Charts = charts;
        }
    }
}
