using DotNet2020.Domain._5.Entities;
using DotNet2020.Domain._5.Services.Interfaces;
using System.Linq;
using YouTrackSharp;

namespace DotNet2020.Domain._5.Services
{
    public class YouTrackService
    {
        private readonly BearerTokenConnection connection;

        public YouTrackService()
        {
            connection = new BearerTokenConnection("https://kpfu-net.myjetbrains.com/youtrack",
                "perm:YW5nZWxh.NTUtMw==.PBKFTDQmoWvoxdzM7t5TPWPtKrTeOI");
        }

        public IssueTimeInfo[] GetIssues(string projectName, string issueFilter = "")
        {
            var issuesService = connection.CreateIssuesService();
            var issues = issuesService.GetIssuesInProject(projectName, filter: issueFilter).Result;
            if (issues == null) return new IssueTimeInfo[0];
            return issues
                .Select(i => new IssueTimeInfo(i.GetField("Estimate")?.AsInt32() / 60, i.GetField("Spent time")?.AsInt32() / 60))
                .ToArray();
        }
    }
}
