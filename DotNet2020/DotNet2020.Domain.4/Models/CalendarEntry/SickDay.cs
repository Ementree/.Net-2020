using System;

namespace DotNet2020.Domain._4.Models
{
    public class SickDay : AbstractCalendarEntry
    {
        public SickDay (DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}
