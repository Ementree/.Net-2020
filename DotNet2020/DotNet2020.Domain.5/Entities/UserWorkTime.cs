using DotNet2020.Data;
using System;

namespace DotNet2020.Domain._5.Entities
{
    public class UserWorkTime
    {
        public AppIdentityUser User { get; private set; }
        public int EstimatedTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public UserWorkTime(AppIdentityUser user, int estimatedTime, DayOfWeek dayOfWeek)
        {
            User = user;
            EstimatedTime = estimatedTime;
            DayOfWeek = dayOfWeek;
        }

        protected UserWorkTime() : base() { }
    }
}
