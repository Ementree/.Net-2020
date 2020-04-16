using System;

namespace DotNet2020.Domain._4.Models
{
    public class SickDay : AbstractCalendarEntry
    {
        public SickDay(DateTime from, DateTime to, string userName)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.SickDay;
            UserName = userName;
        }
    }
}
