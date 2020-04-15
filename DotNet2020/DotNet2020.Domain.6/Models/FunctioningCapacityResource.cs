using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class FunctioningCapacityResource
    {
        public FunctioningCapacityResource(int projectId, int resourceId, int functionCapacity, int periodId)
        {
            ProjectId = projectId;
            ResourceId = resourceId;
            FunctionCapacity = functionCapacity;
            PeriodId = periodId;
        }

        protected FunctioningCapacityResource() : base() { }

        public int Id { get; protected set; }
        public int ProjectId { get; protected set; }
        public int ResourceId { get; protected set; }
        public int FunctionCapacity { get; protected set; }
        public int PeriodId { get; protected set; }
        public virtual Project Project { get; protected set; }
        public virtual Resource Resource { get; protected set; }
        public virtual Period Period { get; protected set; }
    }
}
