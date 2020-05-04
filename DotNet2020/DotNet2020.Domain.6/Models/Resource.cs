using DotNet2020.Data;

namespace DotNet2020.Domain._6.Models
{
    public class Resource
    {
        protected Resource() : base() { }
        public int Id { get; protected set; }
        public int ResourceGroupTypeId { get; protected set; }
        public string AppIdentityUserId { get; protected set; }
        
        public virtual ResourceGroupType ResourceGroupType { get; protected set; }
        public virtual AppIdentityUser AppIdentityUser { get; protected set; }
    }
}
