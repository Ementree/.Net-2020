using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._4.Model
{
    public class Seekday : AbstractCalendarEntry
    {
        public int Id { get; }
        public new DateTime From { get; private set; }
        public new DateTime To { get; private set; }
    }
}
