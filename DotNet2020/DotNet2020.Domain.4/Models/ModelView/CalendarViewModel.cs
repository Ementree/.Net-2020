namespace Kendo.Mvc.Examples.Models.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DotNet2020.Data;
    using Kendo.Mvc.UI;

    public class CalendarEventViewModel : ISchedulerEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }

        private DateTime end;

        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get => true ; set => throw new NotImplementedException(); }
        public string UserEmail { get; set; }
        public string ColorId { get; set; }
    }
}