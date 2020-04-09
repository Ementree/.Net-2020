using System;
using System.Collections.Generic;
using DotNet2020.Data;
using Kendo.Mvc.Examples.Models.Scheduler;

namespace DotNet2020.Domain.Models.ModelView
{
    public class IndexViewModel
    {
        public List<CalendarEventViewModel> Events { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
