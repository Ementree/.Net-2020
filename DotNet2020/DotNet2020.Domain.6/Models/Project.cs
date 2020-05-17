namespace DotNet2020.Domain._6.Models
{
    public class Project
    {
        public Project(string name, int projectStatusId)
        {
            Name = name;
            ProjectStatusId = projectStatusId;
        }
        protected Project() : base() { }
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int ProjectStatusId { get; protected set; }
        public virtual ProjectStatus ProjectStatus { get; protected set; }

        public void UpdateProjectInfo(string newName, int newProjectStatusId)
        {
            Name = newName;
            ProjectStatusId = newProjectStatusId;
        }
    }
}
