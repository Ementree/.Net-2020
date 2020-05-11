using DotNet2020.Domain._5.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._5.Models
{
    public class ChartModel
    {
        public List<Chart> Charts { get; set; }
        public Issue[] Issues { get; set; }
        public Report Report { get; set; }
        public ChartModel() { }
        public ChartModel(List<Chart> charts, Issue[] issuess)
        {
            Issues = issuess;
            Charts = charts.ToList();
        }
    }
}
