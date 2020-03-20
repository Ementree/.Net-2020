using System;

namespace DotNet2020.Domain._4.Models
{
    public class Sickday : AbstractCalendarEntry
    {
        public int Id { get; }
        public new DateTime From { get; private set; }
        public new DateTime To { get; private set; }
    }
}
