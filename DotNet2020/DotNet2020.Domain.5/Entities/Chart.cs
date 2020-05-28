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
        public int Tick { get;set; }

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
            XAxis = new List<double>();
            YAxis = new List<double>();

            if (issues == null || issues.Count == 0)
            {
                _issues = new List<Issue>();
                return;
            }

            _issues = issues;
            var chart = issues
                .Select(_selector)
                .Where(d => d.HasValue)
                .Select(d => d.Value)
                .OrderBy(d => d)
                .ToList();

            if (chart.Count == 0)
                return;

            var dict = new Dictionary<int, int>();

            int first = (int)Math.Floor(chart[0]);
            int last = (int)Math.Ceiling(chart[chart.Count - 1]);

            Tick = (int)Math.Ceiling((double)(last - first) / count);

            int counter = 0;
            for (int i = first; i <= last; i += Tick)
            {
                if (!dict.ContainsKey(i))
                    dict[i] = 0;

                for (; counter < chart.Count && chart[counter] < i + Tick; counter++)
                    dict[i]++;
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
                .Where(i => i.Value.HasValue && i.Value.Value >= start && i.Value.Value < end)
                .Select(i => i.Key)
                .Clone()
                .ToList();

            return issues;
        }
    }
}
