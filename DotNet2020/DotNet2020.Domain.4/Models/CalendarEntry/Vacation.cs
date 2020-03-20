using System;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry
    {
        public bool IsApproved { get; private set; }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
