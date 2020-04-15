using System;
using System.Collections.Generic;
using DotNet2020.Data;
using DotNet2020.Domain._4.Models;
using Kendo.Mvc.Examples.Models.Scheduler;

namespace DotNet2020.Domain.Models.ModelView
{
    public class IndexViewModel
    {
        public List<CalendarEventViewModel> Events { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<string> Holidays { get; set; }
    }
}
