using DotNet2020.Domain._5.Entities;
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
        /// <returns></returns>
        Issue GetIssue(string projectName, string issueName);

        /// <summary>
        /// Get issues by project name and issue filter
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="issueFilter">Issue filter</param>
        /// <returns></returns>
        List<Issue> GetIssues(string projectName, string issueFilter = "");

        /// <summary>
        /// Get all project names
        /// </summary>
        /// <returns></returns>
        string[] GetAllProjects();
    }
}
