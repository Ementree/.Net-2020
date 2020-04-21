using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Issue
    {
        /// <summary>
        /// Issue name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Issue title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Time info
        /// </summary>
        public IssueTimeInfo TimeInfo { get; private set; }

        /// <summary>
        /// Creator name
        /// </summary>
        public string CreatorName { get; private set; }

        /// <summary>
        /// Executor name (can be null)
        /// </summary>
        public string AssigneeName { get; private set; }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Full link
        /// </summary>
        public string Link { get; private set; }

        /// <summary>
        /// Work items
        /// </summary>
        public List<WorkItem> WorkItems { get; private set; }

        public Issue(string name, string title, IssueTimeInfo timeInfo, 
            string creatorName, string assigneeName, string projectName, string link, List<WorkItem> workItems = null)
        {
            CheckIsEmpty(name, "name");
            CheckIsEmpty(title, "title");
            CheckIsEmpty(creatorName, "creatorName");
            CheckIsEmpty(projectName, "projectName");
            CheckIsEmpty(link, "link");

            if (workItems == null)
                workItems = new List<WorkItem>();

            Name = name;
            Title = title;
            TimeInfo = timeInfo;
            CreatorName = creatorName;
            AssigneeName = assigneeName;
            ProjectName = projectName;
            Link = link;
            WorkItems = workItems;
        }

        private void CheckIsEmpty(string parameter, string parameterName)
        {
            if (String.IsNullOrEmpty(parameter))
                throw new ArgumentException("Must be not empty!", parameterName);
        }
    }
}
