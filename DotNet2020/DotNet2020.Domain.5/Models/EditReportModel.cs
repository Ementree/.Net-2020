using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet2020.Domain._5.Models
{
    public class EditReportModel
    {
        [MaxLength(50)]
        public string ReportName { get; set; }

        public string Project { get; set; }

        public string[] UserFilter { get; set; }

        public bool IsAssignee { get; set; }
        public bool IsWorkItems { get; set; }
    }
}

