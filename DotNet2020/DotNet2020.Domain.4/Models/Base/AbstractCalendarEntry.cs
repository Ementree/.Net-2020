using System;
using DotNet2020.Data;
using Kendo.Mvc.UI;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry
    {
        public int Id { get; set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public AbsenceType AbsenceType { get; set; }
        public string UserId { get; set; }
        public AppIdentityUser User { get; set; }

        protected AbstractCalendarEntry(DateTime from, DateTime to, AppIdentityUser user, AbsenceType type)
        {
            From = from;
            To = to;
            AbsenceType = type;
            User = user;
        }
        
        protected AbstractCalendarEntry(){}

        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException("You are trying set incorrect data period!" +
                "'From' parametr should be less than 'To' or equal to them");
            From = from;
            To = to;
        }

        public string UserName
        {
            get
            {
                if (User != null)
                    return $"{User.Employee.FirstName} {User.Employee.LastName}" == " " ? 
                        User.Email : $"{User.Employee.FirstName} {User.Employee.LastName}";
                else return "";
            }
        }
    }
}
