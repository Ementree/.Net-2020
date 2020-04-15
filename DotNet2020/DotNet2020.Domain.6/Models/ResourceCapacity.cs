namespace DotNet2020.Domain._6.Models
{
    public class ResourceCapacity
    {
        public ResourceCapacity(int resourceId, int capacity, int periodId, Resource resource, Period period)
        {
            ResourceId = resourceId;
            Capacity = capacity;
            PeriodId = periodId;
            Resource = resource;
            Period = period;
        }
        protected ResourceCapacity() : base() { }
        public int Id { get; protected set; }
        public int ResourceId { get; protected set; }
        public int Capacity { get; protected set; }
        public int PeriodId { get; protected set; }
        public virtual Resource Resource { get; protected set; }
        public virtual Period Period { get; protected set; }
    }
}
