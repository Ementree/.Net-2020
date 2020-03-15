using System;

namespace DotNet2020.Domain._4.Models
{
    public class Vacation : AbstractCalendarEntry
    {
        public int Id { get; }
        public bool IsApproved { get; private set; }
        public bool IsPaid { get; private set; }

        public void Approve()
        {
            IsApproved = true;
        }
    }
}
