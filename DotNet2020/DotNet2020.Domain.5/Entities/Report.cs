using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public string Name { get; private set; }
        public string ProjectName { get; private set; }
        public string IssueFilter { get; private set; }
        public List<Issue> Issues { get; private set; }

        public Report(string name, string projectName, string issueFilter, List<Issue> issues = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");
            if (projectName == null)
                projectName = String.Empty;
            if (issueFilter == null)
                issueFilter = String.Empty;
            if (issues == null)
                issues = new List<Issue>();

            Name = name;
            ProjectName = projectName;
            IssueFilter = issueFilter;
            Issues = issues;
        }

        protected Report() : base() { }
    }
}
