using System;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }
        public bool IsPaid { get; private set; }

        public Vacation(DateTime from, DateTime to, string userName)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.Vacation;
            UserName = userName;
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
