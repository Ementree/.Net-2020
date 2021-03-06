﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        [NotMapped]
        public double? ErrorCoef => EstimatedTime.HasValue && SpentTime.HasValue && EstimatedTime != 0
            ? (double)SpentTime / EstimatedTime
            : null;

        /// <summary>
        /// Ошибка в часах
        /// </summary>
        [NotMapped]
        public int? ErrorInHours => 
            EstimatedTime.HasValue && SpentTime.HasValue
            ? SpentTime - EstimatedTime
            : null;

        /// <summary>
        /// Установить потраченное время по work items
        /// </summary>
        public void SetTimeByWorkItems()
        {
            if (WorkItems.Count == 0)
                return;

            SpentTime = WorkItems
                .GroupBy(w => w.UserName)
                .Select(g => g.Sum(w => w.SpentTime))
                .Max();
        }
    }
}
