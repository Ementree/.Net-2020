namespace DotNet2020.Domain._6.Models
{
    public class ProjectStatus
    {
        public ProjectStatus(string status)
        {
            Status = status;
        }
        protected ProjectStatus() : base() { }
        public int Id { get; protected set; }
        public string Status { get; protected set; }
    }
}
