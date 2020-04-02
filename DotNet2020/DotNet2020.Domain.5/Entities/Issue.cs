using System;

namespace DotNet2020.Domain._5.Entities
{
    public class Issue
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public IssueTimeInfo TimeInfo { get; private set; }
        public IssueStateInfo StateInfo { get; private set; }

        public User Creator { get; private set; }
        public User Assignee { get; private set; }
        public Project Project { get; private set; }
        public DateTime? DueDate { get; private set; }

        public Issue(string name, string title, string description, User creator, Project project,
            IssueTimeInfo timeInfo = new IssueTimeInfo(), IssueStateInfo stateInfo = new IssueStateInfo(), 
            User assignee = null, DateTime? dueDate = null)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");
            if (String.IsNullOrEmpty(title))
                throw new ArgumentException("Must be not empty!", "Title");
            if (String.IsNullOrEmpty(description))
                throw new ArgumentException("Must be not empty!", "Description");

            Name = name;
            Title = title;
            Description = description;
            TimeInfo = timeInfo;
            StateInfo = stateInfo;
            Creator = creator;
            Assignee = assignee;
            Project = project;
            DueDate = dueDate;
        }
    }
}
