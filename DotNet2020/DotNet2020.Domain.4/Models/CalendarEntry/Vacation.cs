using System;
using System.Linq;
using DotNet2020.Domain._4.Domain;
using DotNet2020.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }
        public bool IsPaid { get; private set; }

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
        #warning убрать DbContext -> инверсия зависимости
        public void Approve(DbContext context)
        {
            var user = context.Set<EmployeeCalendar>()
                .FirstOrDefault(u => u.Id == CalendarEmployeeId);
            if(user == null) throw new NullReferenceException();
            var holidays = context.Set<Holiday>()
                .Where(u => u.Date >= From && u.Date <= To).ToList();
            var days = DomainLogic.GetDatesFromInterval(From, To);
            var total = DomainLogic.GetWorkDay(days, holidays);
            user.TotalDayOfVacation = user.TotalDayOfVacation - total;
            IsApproved = true;
            user.Approve();
            context.SaveChanges();
        }
    }
}
