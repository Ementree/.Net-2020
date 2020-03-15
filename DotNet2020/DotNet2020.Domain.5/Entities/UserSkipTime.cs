using DotNet2020.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._5.Entities
{
    public class UserSkipTime
    {
        public AppIdentityUser User { get; private set; }
        public int SpentInHours { get; set; }
        public DateTime Date { get; set; }
        public SickEnum SickReason { get; set; }

        public UserSkipTime(AppIdentityUser user, int spentInHours, DateTime date, SickEnum sickReason)
        {
            User = user;
            SpentInHours = spentInHours;
            Date = date;
            SickReason = sickReason;
        }

        protected UserSkipTime() : base() { }
    }
}
