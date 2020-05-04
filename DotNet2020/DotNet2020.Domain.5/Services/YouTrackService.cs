﻿using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using YouTrackSharp;

namespace DotNet2020.Domain._5.Services
{
    public class YouTrackService
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

        private Issue CreateIssue(YouTrackSharp.Issues.Issue issue, IEnumerable<YouTrackSharp.TimeTracking.WorkItem> workItems)
        {
            return new Issue(
                issue.Id,
                issue.Summary,
                issue.GetField("Estimate")?.AsInt32() / 60,
                issue.GetField("Spent time")?.AsInt32() / 60,
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
