using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._5.Entities
{
    public class Chart
    {
        private Dictionary<double,int> dict { get; set; }
        public List<double> XAxis { get; set; } 
        public List<double> YAxis { get; set; }
        public double Tick { get;set; }
        public Chart(IEnumerable<double> chart, int count)
        {
            XAxis = new List<double>();
            YAxis = new List<double>();
            dict = new Dictionary<double, int>();
            Tick = Math.Ceiling((chart.Last() - chart.First()) / count);
            var min = chart.First() + Tick;
            foreach (var e in chart)
            {
                if (!dict.ContainsKey(min))
                    dict[min] = 0;
                if (e <= min)
                {
                    dict[min]++;
                }
                else
                {
                    while (true)
                    {
                        min += Tick;
                        if (!dict.ContainsKey(min))
                            dict[min] = 0;
                        if (e <= min)
                        {
                            if (!dict.ContainsKey(min))
                                dict[min] = 0;
                            dict[min]++;
                            break;
                        }
                    }
                }
            }
            foreach (var e in dict)
            {
                XAxis.Add(e.Key);
                YAxis.Add(e.Value);
            }
        }
    }
}
