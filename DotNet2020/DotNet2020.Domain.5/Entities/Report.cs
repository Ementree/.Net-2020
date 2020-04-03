using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Report
    {
        public string Name { get; private set; }
        public string ProjectName { get; private set; }
        public string WorkAuthor { get; private set; }

        public DateTime PeriodFrom { get; private set; }
        public DateTime PeriodTo { get; private set; }

        public List<IssueTimeInfo> Issues { get; }

        public Report(string name, List<IssueTimeInfo> issues)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");
            if (issues == null)
                throw new ArgumentException("Must be not null!", "Issues");

            Name = name;
            Issues = issues;
        }

        public Report(string name, string projectName, string workAuthor, 
            DateTime periodFrom, DateTime periodTo, List<IssueTimeInfo> issues)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");
            if (String.IsNullOrEmpty(projectName))
                throw new ArgumentException("Must be not empty!", "ProjectName");
            if (String.IsNullOrEmpty(workAuthor))
                throw new ArgumentException("Must be not empty!", "WorkAuthor");
            if (issues == null)
                throw new ArgumentException("Must be not null!", "Issues");
            if (periodFrom > periodTo)
                throw new ArgumentException("Period from must be less than period to");

            Name = name;
            ProjectName = projectName;
            WorkAuthor = workAuthor;
            PeriodFrom = periodFrom;
            PeriodTo = periodTo;
            Issues = issues;
        }
    }
}
