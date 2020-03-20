using System;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }

        public Illness(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
