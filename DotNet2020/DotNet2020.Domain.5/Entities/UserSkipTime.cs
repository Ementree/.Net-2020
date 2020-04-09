using DotNet2020.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Entities
{
    public class UserSkipTime : UserBase
    {
        [Required]
        public int SpentInHours { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public SickEnum SickReason { get; set; }

        public UserSkipTime(AppIdentityUser user, int spentInHours, DateTime date, SickEnum sickReason) : base(user)
        {
            if (spentInHours < 0)
                throw new ArgumentException("Should be >= 0", "SpentInHours");
            if (spentInHours > 24)
                throw new ArgumentException("Should be < 24", "SpentInHours");
            SpentInHours = spentInHours;
            Date = date;
            SickReason = sickReason;
        }

        protected UserSkipTime() : base() { }
    }
}
