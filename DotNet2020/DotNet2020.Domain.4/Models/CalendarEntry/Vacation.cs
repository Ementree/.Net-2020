using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain.Core.Models;
using DotNet2020.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }
        public bool IsPaid { get; private set; }
        public Employee Agreeing { get; set; }

        protected Vacation() { }

        public Vacation(DateTime from, DateTime to, EmployeeCalendar user, bool isPaid)
            : base(from, to, user, AbsenceType.Vacation) 
        {
            IsPaid = isPaid;
        }

        public void Pay()
        {
            IsPaid = true;
        }

        #warning добавить согласующего
        public void Approve(List<Holiday> holidays, Employee employee)
        {
            if(CalendarEmployee == null) throw new NullReferenceException();
            var days = DomainLogic.GetDatesFromInterval(From, To);
            var total = DomainLogic.GetWorkDay(days, holidays);
            CalendarEmployee.TotalDayOfVacation = CalendarEmployee.TotalDayOfVacation - total;
            IsApproved = true;
            CalendarEmployee.Approve();
        }
    }
}
