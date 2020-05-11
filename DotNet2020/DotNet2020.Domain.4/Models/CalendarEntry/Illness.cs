using System;
using System.Collections.Generic;
using DotNet2020.Data;
using DotNet2020.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }

        protected Illness() { }

        public Illness(DateTime from, DateTime to, EmployeeCalendar user)
        : base(from, to, user, AbsenceType.Illness) {}

        #warning добавить согласующего
        public void Approve(List<Holiday> holidays)
        {
            IsApproved = true;
        }
    }
}
