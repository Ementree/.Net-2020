using DotNet2020.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Entities
{
    public class UserSkipTime
    {
        [Required]
        public AppIdentityUser User { get; private set; }

        [Required]
        public int SpentInHours { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public SickEnum SickReason { get; set; }

        public UserSkipTime(AppIdentityUser user, int spentInHours, DateTime date, SickEnum sickReason)
        {
            if (user == null)
                throw new ArgumentNullException("User was null!");
            User = user;
            SpentInHours = spentInHours;
            Date = date;
            SickReason = sickReason;
        }

        protected UserSkipTime() : base() { }
    }
}
