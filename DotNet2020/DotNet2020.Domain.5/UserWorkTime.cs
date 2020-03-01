using DotNet2020.Data;
using System;

namespace DotNet2020.Domain._5
{
    public class UserWorkTime
    {
        public AppIdentityUser User { get; }
        public int EstimatedTime { get; }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }

        public UserWorkTime(AppIdentityUser user, int estimatedTime, DateTime dateFrom, DateTime dateTo)
        {
            User = user;
            EstimatedTime = estimatedTime;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        protected UserWorkTime() : base() { }
    }
}
