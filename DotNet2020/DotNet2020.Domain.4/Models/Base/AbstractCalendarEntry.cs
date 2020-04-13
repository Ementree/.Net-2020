using System;
using DotNet2020.Data;
using Kendo.Mvc.UI;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry
    {
        public int Id { get; set; }
        public DateTime From { get; protected set; }
        public DateTime To { get; protected set; }
        public AbsenceType AbsenceType { get; set; }
        public string UserId { get; set; }
        public AppIdentityUser User { get; set; }


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
                return $"{User.FirstName} {User.LastName}" == " " ? User.Email : $"{User.FirstName} {User.LastName}";
            }
        }
    }
}
