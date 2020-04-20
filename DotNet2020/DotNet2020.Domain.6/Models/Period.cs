using System;

namespace DotNet2020.Domain._6.Models
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        protected Period() : base() { }
        public int Id { get; protected set; }
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
    }
}
