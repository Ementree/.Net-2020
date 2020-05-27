using DotNet2020.Domain._5.Entities;
using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Services.Interfaces
{
    public interface ITimeTrackingService
    {
        /// <summary>
        /// Get issue by project name and issue name
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="issueName">Issue name</param>
        Issue GetIssue(string projectName, string issueName);

        /// <summary>
        /// Get issues by project name and issue filter
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="issueFilter">Issue filter</param>
        List<Issue> GetIssues(string projectName, string issueFilter = "");

        /// <summary>
        /// Get all project names
        /// </summary>
        string[] GetAllProjects();

        /// <summary>
        /// Get issues that were tracked by
        /// </summary>
        List<Issue> GetProblemIssues(List<Issue> issues);

        /// <summary>
        /// Get all project users in project
        /// </summary>
        List<string> GetAllUsers(string projectName);

        /// <summary>
        /// Add start date and end date to filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="start">Start date</param>
        /// <param name="end">End date</param>
        /// <returns></returns>
        string AddDateToFilter(string filter, DateTime start, DateTime end);

        /// <summary>
        /// Add assignee to filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <param name="userName">User name</param>
        /// <returns></returns>
        string AddAssigneeToFilter(string filter, string userName);
    }
}
