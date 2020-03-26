using DotNet2020.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Entities
{
    public class UserWorkTime
    {
        [Required]
        public AppIdentityUser User { get; private set; }

        [Required]
        public int EstimatedTime { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        public bool IsVisible { get; set; }

        public UserWorkTime(AppIdentityUser user, int estimatedTime, DayOfWeek dayOfWeek, bool isVisible)
        {
            if (user == null)
                throw new ArgumentNullException("User was null!");
            User = user;
            EstimatedTime = estimatedTime;
            DayOfWeek = dayOfWeek;
            IsVisible = isVisible;
        }

        protected UserWorkTime() : base() { }
    }
}
