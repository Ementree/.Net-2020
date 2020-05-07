using System;
using DotNet2020.Data;

namespace DotNet2020.Domain._4.Models
{
    public class SickDay : AbstractCalendarEntry
    {
        protected SickDay() { }

        public SickDay(DateTime from, DateTime to, AppIdentityUser user)
            : base(from, to, user, AbsenceType.SickDay) {}
    }
}
