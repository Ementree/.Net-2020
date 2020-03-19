using System;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }

        //public Vacation(DateTime from, DateTime to)
        //{
        //    this.From = from;
        //    this.To = to;
        //}

        //public void Approve()
        //{
        //    IsApproved = true;
        //}
    }
}
