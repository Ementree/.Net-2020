﻿using System;
using System.Collections.Generic;
using DotNet2020.Domain.Core.Models;
using DotNet2020.Domain.Models;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }
        public Employee Agreeing { get; set; }


        protected Illness() { }

        public Illness(DateTime from, DateTime to, EmployeeCalendar user)
        : base(from, to, user, AbsenceType.Illness) {}

        #warning добавить согласующего
        public void Approve(List<Holiday> holidays, Employee agreeing)
        {
            Agreeing = agreeing;
            IsApproved = true;
        }
    }
}
