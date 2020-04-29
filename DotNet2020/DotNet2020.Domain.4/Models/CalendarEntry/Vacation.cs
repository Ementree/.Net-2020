using System;
using System.Linq;
using DotNet2020.Data;
using DotNet2020.Domain._4.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }
        public bool IsPaid { get; private set; }

        protected Vacation() { }

        public Vacation(DateTime from, DateTime to, AppIdentityUser user)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.Vacation;
            User = user;
            IsPaid = true;
        }

        public void Pay()
        {
            IsPaid = true;
        }

        #warning добавить согласующего
        public void Approve(DbContext context)
        {
            var user = context.Set<AppIdentityUser>()
                .FirstOrDefault(u => u.Id == UserId);
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
