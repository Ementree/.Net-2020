using DotNet2020.Domain.Core.Models;

namespace DotNet2020.Domain._6.Models
{
    public class Resource
    {
        protected Resource() : base() { }
        public int Id { get; protected set; }
        public int ResourceGroupTypeId { get; protected set; }
        public int EmployeeId { get; protected set; }
        
        public virtual ResourceGroupType ResourceGroupType { get; protected set; }
        public virtual Employee Employee { get; protected set; }

        public void UpdateGroupAndType(int id)
        {
            ResourceGroupTypeId = id;
        }
    }
}
