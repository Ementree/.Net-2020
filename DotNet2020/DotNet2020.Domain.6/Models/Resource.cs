namespace DotNet2020.Domain._6.Models
{
    public class Resource
    {
        public Resource(string firstName, string lastName, int resourceGroupTypeId, ResourceGroupType resourceGroupType)
        {
            FirstName = firstName;
            LastName = lastName;
            ResourceGroupTypeId = resourceGroupTypeId;
            ResourceGroupType = resourceGroupType;
        }
        protected Resource() : base() { }
        public int Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public int ResourceGroupTypeId { get; protected set; }
        public virtual ResourceGroupType ResourceGroupType { get; protected set; }
    }
}
