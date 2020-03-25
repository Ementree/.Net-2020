using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class ResourceCapacity
    {
        public int Id { get; protected set; }
        public int ResourceId { get; protected set; }
        public int Capacity { get; protected set; }
        public int PeriodId { get; protected set; }
    }
}
