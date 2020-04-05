using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._4.Models;
using Kendo.Mvc.Examples.Models.Scheduler;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain.Controllers
{
    public class SchedulerController : Controller
    {
        public SchedulerController()
        {
        }

        [HttpPost]
        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = new List<CalendarEventViewModel>()
            {
                new CalendarEventViewModel()
                {
                    MeetingID = 1,
                    Title = "John Snow",
                    Description = "Soft-Engeneer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 8),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 1,
                    Attendees = new List<int> {1, 2, 3}
                },
                new CalendarEventViewModel()
                {
                    MeetingID = 2,
                    Title = "Sara James",
                    Description = "Soft-Engeneer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 7),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 2,
                    Attendees = new List<int> {1, 2, 3}
                },
                new CalendarEventViewModel()
                {
                    MeetingID = 3,
                    Title = "Susan Susan",
                    Description = "Developer",
                    Start = new DateTime(2020, 4, 6),
                    StartTimezone = "Etc/UTC",
                    End = new DateTime(2020, 4, 8),
                    EndTimezone = "Etc/UTC",
                    IsAllDay = true,
                    RoomID = 3,
                    Attendees = new List<int> {1, 2, 3}
                }
            }.AsQueryable();

            return Json(resultList.ToDataSourceResult(request));
        }

        public virtual JsonResult Destroy([DataSourceRequest] DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
