using System;

namespace DotNet2020.Domain._6.Models
{
    public class CalendarEntry
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserName { get; set; }
    }
}
