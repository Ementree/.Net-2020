using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Entities
{
    public class Chart
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<double> XAxis { get; private set; } 
        public List<double> YAxis { get; private set; }
        public double Tick { get;set; }

        private Func<Issue, double?> _selector { get; set; }
        private List<Issue> _issues { get; set; }

        public Chart(int id, string name, Func<Issue, double?> selector)
        {
            Id = id;
            Name = name;
            _selector = selector;
        }

        public void SetData(List<Issue> issues, int count)
        {
            _issues = issues;
            var chart = issues
                .Select(_selector)
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .OrderBy(d => d);

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

        public List<Issue> GetIssues(int start, int end)
        {
            var issues = _issues.ToDictionary(i => i, _selector)
                .Where(i => i.Value.HasValue && i.Value.Value >= start && i.Value.Value <= end)
                .Select(i => i.Key)
                .Clone()
                .ToList();

            return issues;
        }
    }
}
