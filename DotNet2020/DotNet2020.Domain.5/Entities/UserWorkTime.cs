using DotNet2020.Data;
using System;

namespace DotNet2020.Domain._5.Entities
{
    public class UserWorkTime
    {
        public AppIdentityUser User { get; private set; }
        public int EstimatedTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool IsVisible { get; set; }

        public UserWorkTime(AppIdentityUser user, int estimatedTime, DayOfWeek dayOfWeek, bool isVisible)
        {
            User = user;
            EstimatedTime = estimatedTime;
            DayOfWeek = dayOfWeek;
            IsVisible = isVisible;
        }

        protected UserWorkTime() : base() { }
    }
}
