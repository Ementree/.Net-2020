using System;
using System.Collections.Generic;
using DotNet2020.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public interface IApprovableEvent
    {
        public Employee Agreeing { get; set; }
        void Approve(List<Holiday> holidays, Employee employee);
    }
}