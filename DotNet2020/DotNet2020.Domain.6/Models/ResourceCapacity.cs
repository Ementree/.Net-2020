namespace DotNet2020.Domain._6.Models
{
    public class ResourceCapacity
    {
        public ResourceCapacity(int resourceId, int capacity, int periodId)
        {
            ResourceId = resourceId;
            Capacity = capacity;
            PeriodId = periodId;
        }
        protected ResourceCapacity() : base() { }
        public int Id { get; protected set; }
        public int ResourceId { get; protected set; }
        public double Capacity { get; set; }
        public int PeriodId { get; protected set; }
        public virtual Resource Resource { get; protected set; }
        public virtual Period Period { get; protected set; }
    }
}
