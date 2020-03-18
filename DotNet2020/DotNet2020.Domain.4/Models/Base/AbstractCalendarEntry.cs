using System;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public AbsenceType AbsenceType { get; set; }


        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException();
            From = from;
            To = to;
        }
    }
}
