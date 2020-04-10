using System;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }

        public Illness(DateTime from, DateTime to, string userName)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.Illness;
            UserName = userName;
        }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
