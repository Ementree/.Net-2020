using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._5.Services
{
    public class ChartService : IChartService
    {
        private static Dictionary<int, Chart> _charts;

        static ChartService()
        {
            _charts = new Dictionary<int, Chart>();

            _charts.Add(1, new Chart(1, "Запланированное время",
                i => i.EstimatedTime));

            _charts.Add(2, new Chart(2, "Потраченное время", 
                i => i.SpentTime));

            _charts.Add(3, new Chart(3, "Ошибка в процентах",
                i => i.GetErrorCoef()));

            _charts.Add(4, new Chart(4, "Ошибка в часах",
                i => (double)i.GetErrorHours()));

        }

        public Dictionary<int, Chart> GetAllCharts()
        {
            return _charts.Clone();
        }

        public Chart GetChart(int id)
        {
            if (!_charts.ContainsKey(id))
                return null;
            return _charts[id];
        }
    }
}
