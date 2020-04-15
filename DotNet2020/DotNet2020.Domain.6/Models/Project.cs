namespace DotNet2020.Domain._6.Models
{
    public class Project
    {
        public Project(string name, int statusId, ProjectStatus projectStatus)
        {
            Name = name;
            StatusId = statusId;
            ProjectStatus = projectStatus;
        }
        protected Project() : base() { }
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int StatusId { get; protected set; }
        public virtual ProjectStatus ProjectStatus { get; protected set; }
    }
}
