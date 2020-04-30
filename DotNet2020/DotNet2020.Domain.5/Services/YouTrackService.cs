using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using YouTrackSharp;

namespace DotNet2020.Domain._5.Services
{
    public class YouTrackService : ITimeTrackingService
    {
        private readonly BearerTokenConnection connection;
        private readonly string serverUrl;
        private readonly YouTrackSharp.Issues.IIssuesService issueService;
        private readonly YouTrackSharp.TimeTracking.ITimeTrackingService timeService;
        private readonly YouTrackSharp.Projects.IProjectsService projectService;

        public YouTrackService()
        {
            serverUrl = "https://kpfu-net.myjetbrains.com/youtrack";
            connection = new BearerTokenConnection(serverUrl,
                "perm:YW5nZWxh.NTUtMw==.PBKFTDQmoWvoxdzM7t5TPWPtKrTeOI");
            issueService = connection.CreateIssuesService();
            timeService = connection.CreateTimeTrackingService();
            projectService = connection.CreateProjectsService();
        }

        public Issue[] FilterIssues(Issue[] issues, string issueFilter)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetAllProjects()
        {
            var projects = projectService.GetAccessibleProjects().Result;
            if (projects == null) return new string[0];
            return projects.Select(p => p.Name).ToArray();
        }

        public Issue GetIssue(string projectName, string issueName)
        {
            var issue = issueService.GetIssuesInProject(projectName, filter: issueName).Result.FirstOrDefault();
            if (issue == null) return null;
            var workItems = timeService.GetWorkItemsForIssue(issue.Id).Result;
            return CreateIssue(issue, workItems);
        }

        public Issue[] GetIssues(string projectName, string issueFilter = "")
        {
            var issues = issueService.GetIssuesInProject(projectName, issueFilter).Result;
            return issues
                .Select(i => CreateIssue(i, timeService.GetWorkItemsForIssue(i.Id).Result))
                .ToArray();
        }

        public IssueTimeInfo[] GetIssuesTimeInfo(string projectName, string issueFilter = "")
        {
            var issues = issueService.GetIssuesInProject(projectName, filter: issueFilter).Result;
            if (issues == null) return new IssueTimeInfo[0];
            return issues
                .Select(i => new IssueTimeInfo(i.GetField("Estimate")?.AsInt32() / 60, i.GetField("Spent time")?.AsInt32() / 60))
                .ToArray();
        }

        private Issue CreateIssue(YouTrackSharp.Issues.Issue issue, IEnumerable<YouTrackSharp.TimeTracking.WorkItem> workItems)
        {
            return new Issue(
                issue.Id,
                issue.Summary,
                new IssueTimeInfo(
                    issue.GetField("Estimate")?.AsInt32() / 60,
                    issue.GetField("Spent time")?.AsInt32() / 60),
                issue.GetField("reporterFullName").Value.ToString(),
                issue.GetField("Assignee")?.AsCollection().FirstOrDefault(),
                issue.GetField("projectShortName").AsString(),
                serverUrl + @"/issue/" + issue.GetField("projectShortName").AsString(),
                workItems?
                    .Select(w => new WorkItem()
                    {
                        UserName = w.Author.Login,
                        SpentTime = (int)w.Duration.TotalHours
                    })
                    .ToList());
        }
    }
}
