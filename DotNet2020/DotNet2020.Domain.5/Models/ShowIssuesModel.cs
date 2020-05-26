using DotNet2020.Domain._5.Entities;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Models
{
    public class ShowIssuesModel
    {
        public ShowIssuesModel()
        {
            Issues = new List<Issue>();
        }

        public List<Issue> Issues { get; set; }
        public string OrderBy { get; set; }
        public string OrderByDescending { get; set; }
        public string SerializedIssues { get; set; }
        public int ReportId { get; set; }
    }
}
