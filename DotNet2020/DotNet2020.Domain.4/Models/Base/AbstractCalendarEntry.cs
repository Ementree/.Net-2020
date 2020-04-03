using System;
using Kendo.Mvc.UI;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry : ISchedulerEvent
    {
        public int Id { get; set; }
        public DateTime From { get; protected set; }
        public DateTime To { get; protected set; }
        public AbsenceType AbsenceType { get; set; }
        public string UserName { get; set; }

        public string Title { get => UserName; set => UserName = value; }
        public string Description { get => ""; set => throw new NotImplementedException(); }
        public bool IsAllDay { get => true; set => throw new NotImplementedException(); }
        public DateTime Start { get => From; set => From = value; }
        public DateTime End { get => To; set => To = value; }
        public string StartTimezone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EndTimezone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RecurrenceRule { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RecurrenceException { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException("You are trying set incorrect data period!" +
                "'From' parametr should be less than 'To' or equal to them");
            From = from;
            To = to;
        }
    }
}
