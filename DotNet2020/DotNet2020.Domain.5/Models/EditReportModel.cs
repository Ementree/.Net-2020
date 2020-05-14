using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Models
{
    public class EditReportModel
    {
        public int ReportId { get; set; }
        [MaxLength(50)]
        public string ReportName { get; set; }
        public string ProjectName { get; set; }
        public string[] UserName { get; set; }
        public string[] UserFilter { get; set; }
        public bool IsAssignee { get; set; }
        public bool IsWorkItems { get; set; }
    }
}

