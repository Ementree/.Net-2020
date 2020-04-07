using System;
using DotNet2020.Data;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }

        public Illness(DateTime from, DateTime to, AppIdentityUser user)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.Illness;
            User = user;
        }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
