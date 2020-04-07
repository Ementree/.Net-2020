﻿using System;
using DotNet2020.Data;

namespace DotNet2020.Domain._4.Models
{
    public class SickDay : AbstractCalendarEntry
    {
        public SickDay(DateTime from, DateTime to, AppIdentityUser user)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.SickDay;
            User = user;
        }
    }
}
