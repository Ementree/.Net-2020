using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Models
{
    public class EditReportModel
    {
        public int ReportId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ReportName { get; set; }
        [MaxLength(1000)]
        public string IssueFilter { get; set; }
        public string ProjectName { get; set; }
        public List<string> AllUsers { get; set; }
        public string SelectedUser { get; set; }
        public bool IsAssignee { get; set; }
        public bool IsWorkItems { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}

