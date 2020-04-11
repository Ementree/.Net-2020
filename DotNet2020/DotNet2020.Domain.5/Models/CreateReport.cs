using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet2020.Domain._5.Models
{
    public class CreateReport
    {
        [Required] 
        [MaxLength(50)]
        public string ReportName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Project { get; set; }
        [MaxLength(1000)]
        public string IssueFilter { get; set; }
    }
}
