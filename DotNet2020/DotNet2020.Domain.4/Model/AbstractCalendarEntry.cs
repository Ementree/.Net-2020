using System;
namespace DotNet2020.Domain._4.Model
{
    public abstract class AbstractCalendarEntry
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public void ChabgeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException();
            From = from;
            To = to;
        }
    }
}
