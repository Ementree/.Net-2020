using System;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry
    {
        public int UserId { get; protected set; }
        public DateTime From { get; protected set; }
        public DateTime To { get; protected set; }

        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException();
            From = from;
            To = to;
        }

        public bool IsValid()
        {
            if (To < From) return false;
            return true;
        }
    }
}
