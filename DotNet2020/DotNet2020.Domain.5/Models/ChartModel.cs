using DotNet2020.Domain._5.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._5.Models
{
    public class ChartModel
    {
        public Chart Chart1 { get; set; }
        public Chart Chart2 { get; set; }
        public Chart Chart3 { get; set; }
        public Chart Chart4 { get; set; }
        public Issue[] Issues { get; set; }
        public Report Report { get; set; }
        public ChartModel() { }
        public ChartModel(Chart chart1, Chart chart2, Chart chart3, Chart chart4, Issue[] issuess)
        {
            Issues = issuess;
            Chart1 = chart1;
            Chart2 = chart2;
            Chart3 = chart3;
            Chart4 = chart4;
        }
    }
}
