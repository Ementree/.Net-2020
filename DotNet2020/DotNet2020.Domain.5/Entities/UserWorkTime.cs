using DotNet2020.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._5.Entities
{
    public class UserWorkTime : UserBase
    {
        [Required]
        public int EstimatedTime { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        public bool IsVisible { get; set; }

        public UserWorkTime(AppIdentityUser user, int estimatedTime, DayOfWeek dayOfWeek, bool isVisible) : base(user)
        {
            if (estimatedTime < 0)
                throw new ArgumentException("Should be >= 0", "EstimatedTime");
            if (estimatedTime > 24)
                throw new ArgumentException("Should be < 24", "EstimatedTime");
            EstimatedTime = estimatedTime;
            DayOfWeek = dayOfWeek;
            IsVisible = isVisible;
        }

        protected UserWorkTime() : base() { }
    }
}
