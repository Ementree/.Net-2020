using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public interface IApprovableEvent
    {
        void Approve(List<Holiday> holidays);
    }
}