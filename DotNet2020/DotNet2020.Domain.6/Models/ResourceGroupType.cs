namespace DotNet2020.Domain._6.Models
{
    public class ResourceGroupType
    {
        public ResourceGroupType(string type,string group)
        {
            Type = type;
            Group = group;
        }
        protected ResourceGroupType() : base() { }
        public int Id { get; protected set; }
        public string Type { get; protected set; }
        public string Group { get; protected set; }
    }
}
