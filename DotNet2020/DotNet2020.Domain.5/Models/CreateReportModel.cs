﻿using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Models
{
    public class CreateReportModel
    {
        [Required] 
        [MaxLength(50)]
        public string ReportName { get; set; }

        public string[] Project { get; set; }

        [MaxLength(1000)]
        public string IssueFilter { get; set; }
    }
}
