using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Models
{
    public class CreateReportModel
    {
        [Required] 
        [MaxLength(50)]
        public string ReportName { get; set; }
        public string ProjectName { get; set; }
        public string[] CreateProject { get; set; }

        [MaxLength(1000)]
        public string IssueFilter { get; set; }
    }
}
