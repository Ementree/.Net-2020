using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    class FunctioningCapacityProject
    {
        public int Id { get; protected set; }
        public int ProjectId { get; protected set; }
        public int PeriodId { get; protected set; }
        public int FunctioningCapacity { get; protected set; }
    }
}
