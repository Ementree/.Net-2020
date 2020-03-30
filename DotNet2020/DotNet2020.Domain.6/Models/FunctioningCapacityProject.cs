﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class FunctioningCapacityProject
    {
        public int Id { get; protected set; }
        public int ProjectId { get; protected set; }
        public int PeriodId { get; protected set; }
        public int FunctioningCapacity { get; protected set; }
        public virtual Project Project { get; protected set; }
        public virtual Period Period { get; protected set; }
    }
}
