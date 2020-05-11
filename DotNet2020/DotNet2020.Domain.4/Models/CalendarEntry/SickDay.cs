using System;
using DotNet2020.Data;
using DotNet2020.Domain.Models;

namespace DotNet2020.Domain._4.Models
{
    public class SickDay : AbstractCalendarEntry
    {
        protected SickDay() { }

        public SickDay(DateTime from, DateTime to, EmployeeCalendar user)
            : base(from, to, user, AbsenceType.SickDay) {}
    }
}
