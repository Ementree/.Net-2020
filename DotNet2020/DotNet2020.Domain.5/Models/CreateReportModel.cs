using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Models
{
    public class CreateReportModel
    {
        [Required] 
        [MaxLength(100)]
        public string ReportName { get; set; }
        public string ProjectName { get; set; }
        public string[] ProjectNames { get; set; }
        [MaxLength(1000)]
        public string IssueFilter { get; set; }
        public string UserName { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public string[][] UsersInProjects { get; set; }
    }
}
