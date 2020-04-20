using System;
using DotNet2020.Data;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry
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

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
