using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._5.Entities
{
    public class Chart
    {
        public string Name { get; private set; }
        public List<double> XAxis { get; private set; } 
        public List<double> YAxis { get; private set; }
        public double Tick { get;set; }
        public Chart(IEnumerable<double> chart,string name, int count)
        {
            Name = name;
            XAxis = new List<double>();
            YAxis = new List<double>();
            var dict = new Dictionary<double, int>();
            Tick = Math.Ceiling((chart.Last() - chart.First()) / count);
            var min = chart.First() + Tick;
            foreach (var e in chart)
            {
                if (!dict.ContainsKey(min))
                    dict[min] = 0;
                while(e>min)
                {
                    min += Tick;
                    if (!dict.ContainsKey(min))
                        dict[min] = 0;
                }
                dict[min]++;
            }
            foreach (var e in dict)
            {
                XAxis.Add(e.Key);
                YAxis.Add(e.Value);
            }
        }
    }
}
