using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._5.Models
{
    public class CreateReport
    {
        public string ReportName { get; set; }
        public string Project { get; set; }
        public string IssueFilter { get; set; }
    }
}
