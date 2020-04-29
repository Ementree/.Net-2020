using System;
using DotNet2020.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Models
{
    public class Illness : AbstractCalendarEntry, IApprovableEvent
    {
        public bool IsApproved { get; private set; }

        protected Illness() { }

        public Illness(DateTime from, DateTime to, AppIdentityUser user)
        {
            From = from;
            To = to;
            AbsenceType = AbsenceType.Illness;
            User = user;
        }

        #warning добавить согласующего
        public void Approve(DbContext context)
        {
            IsApproved = true;
            context.SaveChanges();
        }
    }
}
