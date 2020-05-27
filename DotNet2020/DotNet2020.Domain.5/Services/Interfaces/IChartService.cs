using DotNet2020.Domain._5.Entities;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Services.Interfaces
{
    public interface IChartService
    {
        Dictionary<int, Chart> GetAllCharts();
        Chart GetChart(int id);
    }
}
