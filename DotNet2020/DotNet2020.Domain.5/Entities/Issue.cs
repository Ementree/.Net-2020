using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Issue
    {
        /// <summary>
        /// Issue id (PK for EF)
        /// </summary>
        public int IssueId { get; set; }

        /// <summary>
        /// Report id (FK for EF)
        /// </summary>
        public int ReportId { get; set; }
        
        /// <summary>
        /// Issue name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Issue title
        /// </summary>
        public string Title { get; private set; }

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

        public int? EstimatedTime { get; private set; }
        public int? SpentTime { get; private set; }

        public Issue(string name, string title, int? estimatedTime, int? spentTime, 
            string creatorName, string assigneeName, string projectName, string link, List<WorkItem> workItems = null)
        {
            CheckIsEmpty(name, "name");
            CheckIsEmpty(title, "title");
            CheckIsEmpty(creatorName, "creatorName");
            CheckIsEmpty(projectName, "projectName");
            CheckIsEmpty(link, "link");

            if (workItems == null)
                workItems = new List<WorkItem>();

            if (estimatedTime.HasValue && estimatedTime < 0)
                throw new ArgumentException("Must be equal to or greater than 0!", "EstimatedTime");
            if (spentTime.HasValue && spentTime < 0)
                throw new ArgumentException("Must be equal to or greater than 0!", "SpentTime");

            EstimatedTime = estimatedTime;
            SpentTime = spentTime;

            Name = name;
            Title = title;
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

        protected Issue() : base() { }

        // <summary>
        /// Получить коэффициент ошибки
        /// </summary>
        public double? GetErrorCoef()
        {
            return EstimatedTime.HasValue && SpentTime.HasValue
                ? (double)SpentTime / EstimatedTime
                : null;
        }

        /// <summary>
        /// Получить ошибку в часах
        /// </summary>
        public int? GetErrorHours()
        {
            return EstimatedTime.HasValue && SpentTime.HasValue
                ? SpentTime - EstimatedTime
                : null;
        }
    }
}
