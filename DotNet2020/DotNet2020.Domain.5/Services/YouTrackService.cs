using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
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

        public YouTrackService(IConfiguration configuration)
        {
            serverUrl = configuration.GetConnectionString("YouTrackUrl");
            connection = new BearerTokenConnection(serverUrl,
                configuration.GetConnectionString("YouTrackToken"));
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

        public List<string> GetAllUsers(string projectName)
        {
            if (projectName == null) 
                return new List<string>();
            var project = projectService.GetAccessibleProjects(true).Result.Where(p => p.Name == projectName).FirstOrDefault();
            if (project == null) 
                return new List<string>();
            return project.AssigneesLogin.Select(x => x.Value).ToList();
        }

        public Issue GetIssue(string projectName, string issueName)
        {
            if (projectName == null || issueName == null) return null;
            var issue = issueService.GetIssuesInProject(projectName, filter: issueName).Result.FirstOrDefault();
            if (issue == null) return null;
            var workItems = timeService.GetWorkItemsForIssue(issue.Id).Result;
            return CreateIssue(issue);
        }

        public List<Issue> GetIssues(string projectName, string issueFilter = "")
        {
            if (projectName == null) return new List<Issue>();
            if (issueFilter == null) issueFilter = "";
            var issues = issueService.GetIssuesInProject(projectName, filter: issueFilter, take: 10000).Result;
            if (issues == null) return new List<Issue>();
            return issues
                .Select(i => CreateIssue(i))
                .ToList();
        }

        public List<Issue> GetProblemIssues(List<Issue> issues)
        {
            if (issues == null) return new List<Issue>();
            return issues
                .Select(x => Tuple.Create(x, x.WorkItems
                    .GroupBy(a => a.UserName)))
                .Where(x => x.Item2.Count() > 1)
                .Select(x => x.Item1)
                .Concat(issues
                    .Select(i => Tuple.Create(i,
                        issueService.GetChangeHistoryForIssue(i.Name).Result ?? new List<YouTrackSharp.Issues.Change>()))
                    .Where(x => WasChangedInProgress(x.Item2))
                    .Select(x => x.Item1))
                .Distinct()
                .ToList();
        }

        private bool WasChangedInProgress(IEnumerable<YouTrackSharp.Issues.Change> changes)
        {
            var isInProgress = false;
            foreach (var change in changes)
                foreach (var field in change.Fields)
                {
                    if (field.From.Name == "State" && (string)((JArray)field.To.Value).First == "In Progress")
                        isInProgress = true;
                    if (field.From.Name == "State" && (string)((JArray)field.From.Value).First == "In Progress")
                        isInProgress = false;
                    if (isInProgress && (field.Name == "Estimate" || field.Name == "link")) return true;
                }
            return false;
        }

        private Issue CreateIssue(YouTrackSharp.Issues.Issue issue)
        {
            var workItems = timeService.GetWorkItemsForIssue(issue.Id).Result;
            return new Issue(
                issue.Id,
                issue.Summary,
                issue.GetField("Estimate")?.AsInt32() / 60,
                issue.GetField("Spent time")?.AsInt32() / 60,
                issue.GetField("reporterFullName").Value.ToString(),
                ((List<YouTrackSharp.Issues.Assignee>)issue.GetField("Assignee")?.Value)?.FirstOrDefault()?.UserName,
                issue.GetField("projectShortName").AsString(),
                serverUrl + @"/issue/" + issue.Id,
                issue.GetField("created").AsDateTime(),
                workItems?
                    .Select(w => new WorkItem()
                    {
                        UserName = w.Author.Login,
                        SpentTime = (int)w.Duration.TotalHours
                    })
                    .ToList());
        }

        public string AddDateToFilter(string filter, DateTime? start, DateTime? end)
        {
            if (start == null && end == null)
                return RemoveProperty(filter, "created");
            if (start != null && end != null)
            {
                var startStr = String.Format("{0:yyyy-MM-dd}", start);
                var endStr = String.Format("{0:yyyy-MM-dd}", end);
                return AddOrChangeProperty(filter, "created", $"{startStr} .. {endStr}");
            }
            var date = String.Format("{0:yyyy-MM-dd}", start ?? end);
            return AddOrChangeProperty(filter, "created", $"{date}");
        }

        public string AddAssigneeToFilter(string filter, string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return RemoveProperty(filter, "assignee");
            return AddOrChangeProperty(filter, "assignee", userName);
        }

        public (DateTime? start, DateTime? end) GetDateRangeFromFilter(string filter)
        {
            var created = GetPropertyValue(filter, "created");
            if (String.IsNullOrEmpty(created))
                return (null, null);
            var dateArray = created.Split("..");
            if (dateArray.Length == 1)
            {
                var date = DateTime.ParseExact(dateArray[0].Trim(), "yyyy-MM-dd", null);
                return (date, date);
            }
            var start = DateTime.ParseExact(dateArray[0].Trim(), "yyyy-MM-dd", null);
            var end = DateTime.ParseExact(dateArray[1].Trim(), "yyyy-MM-dd", null);
            return (start, end);
        }

        public string GetAssingeeFromFilter(string filter)
        {
            return GetPropertyValue(filter, "assignee");
        }

        private string AddOrChangeProperty(string filter, string propertyName, string value)
        {
            if (String.IsNullOrEmpty(filter))
                return $"{propertyName}: {value}";
            if (filter.Contains(propertyName))
                filter = RemoveProperty(filter, propertyName);
            return $"{filter} {propertyName}: {value}";
        }

        private string RemoveProperty(string filter, string propertyName)
        {
            int startIndex = filter.IndexOf(propertyName);
            if (startIndex < 0)
                return filter;
            int propertyLength = propertyName.Length + 1;
            int endIndex = GetEndIndex(filter, startIndex, propertyLength);

            // Remove property
            int count = propertyLength + endIndex;
            if (count > filter.Length - startIndex)
                count = filter.Length - startIndex;
            return filter.Remove(startIndex, count);
        }

        private string GetPropertyValue(string filter, string propertyName)
        {
            int startIndex = filter.IndexOf(propertyName);
            if (startIndex < 0)
                return null;
            int propertyLength = propertyName.Length + 1;
            int endIndex = GetEndIndex(filter, startIndex, propertyLength);

            // Remove property
            int count = endIndex;
            if (count > filter.Length - startIndex)
                count = filter.Length - startIndex - propertyLength;
            return filter.Substring(startIndex + propertyLength, count).Trim();
        }

        private static int GetEndIndex(string filter, int startIndex, int propertyLength)
        {
            // Find nearest separating symbol (':' or '#')
            string filterAfterProperty = filter.Substring(startIndex + propertyLength);
            int colonIndex = filterAfterProperty.IndexOf(':');
            if (colonIndex > 0)
                colonIndex -= new string(filterAfterProperty.Substring(0, colonIndex).Reverse().ToArray()).IndexOf(' ');
            int tagIndex = filterAfterProperty.IndexOf('#');
            if (tagIndex < 0)
                tagIndex = int.MaxValue / 2;
            if (colonIndex < 0)
                colonIndex = int.MaxValue / 2;
            return Math.Min(tagIndex, colonIndex);
        }
    }
}
