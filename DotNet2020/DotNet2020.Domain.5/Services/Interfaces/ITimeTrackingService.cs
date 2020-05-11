using DotNet2020.Domain._5.Entities;

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
        Issue[] GetIssues(string projectName, string issueFilter = "");

        /// <summary>
        /// Get all project names
        /// </summary>
        string[] GetAllProjects();

        /// <summary>
        /// Get all project users in project
        /// </summary>
        string[] GetAllUsers(string projectName);
    }
}
