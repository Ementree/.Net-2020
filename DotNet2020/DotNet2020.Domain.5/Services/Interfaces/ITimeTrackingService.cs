using DotNet2020.Domain._5.Entities;

namespace DotNet2020.Domain._5.Services.Interfaces
{
    public interface ITimeTrackingService
    {
        /// <summary>
        /// Get issues by project name and issue filter
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <param name="issueFilter">Issue filter</param>
        /// <returns>Issue time info array</returns>
        IssueTimeInfo[] GetIssues(string projectName, string issueFilter);
    }
}
