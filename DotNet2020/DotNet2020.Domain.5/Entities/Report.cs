﻿using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Report
    {
        public int ReportId { get; private set; }
        public string Name { get; private set; }
        public string ProjectName { get; private set; }
        public string IssueFilter { get; private set; }
        public List<IssueTimeInfo> Issues { get; }

        public Report(string name, string projectName, string issueFilter)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");
            if (projectName == null)
                projectName = String.Empty;
            if (issueFilter == null)
                issueFilter = String.Empty;

            Name = name;
            ProjectName = projectName;
            IssueFilter = issueFilter;
            Issues = new List<IssueTimeInfo>();
        }

        protected Report() : base() { }
    }
}
