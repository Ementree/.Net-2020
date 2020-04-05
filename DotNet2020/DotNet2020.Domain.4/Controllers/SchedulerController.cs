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
            List<Projection> cinemaSchedule = new List<Projection> {
            new Projection {
            Title = "Fast and furious 6",
            Start = new DateTime(2013,6,13,17,00,00),
            End= new DateTime(2013,6,13,18,30,00)
            },
            new Projection {
            Title= "The Internship",
            Start= new DateTime(2013,6,13,14,00,00),
            End= new DateTime(2013,6,13,15,30,00)
            },
            new Projection {
            Title = "The Perks of Being a Wallflower",
            Start =  new DateTime(2013,6,13,16,00,00),
            End =  new DateTime(2013,6,13,17,30,00)
            }};

            return Json(cinemaSchedule.ToDataSourceResult(request));
            //var resultList = new List<MeetingViewModel>()
            //{
            //    new MeetingViewModel()
            //    {
            //        MeetingID = 1,
            //        Title = "John Snow",
            //        Description = "Soft-Engeneer",
            //        Start = new DateTime(2020, 4, 6),
            //        StartTimezone = "Etc/UTC",
            //        End = new DateTime(2020, 4, 8),
            //        EndTimezone = "Etc/UTC",
            //        IsAllDay = true,
            //        RoomID = 1,
            //        Attendees = new List<int> {1, 2, 3}
            //    },
            //    new MeetingViewModel()
            //    {
            //        MeetingID = 2,
            //        Title = "Sara James",
            //        Description = "Soft-Engeneer",
            //        Start = new DateTime(2020, 4, 6),
            //        StartTimezone = "Etc/UTC",
            //        End = new DateTime(2020, 4, 7),
            //        EndTimezone = "Etc/UTC",
            //        IsAllDay = true,
            //        RoomID = 2,
            //        Attendees = new List<int> {1, 2, 3}
            //    },
            //    new MeetingViewModel()
            //    {
            //        MeetingID = 3,
            //        Title = "Susan Susan",
            //        Description = "Developer",
            //        Start = new DateTime(2020, 4, 6),
            //        StartTimezone = "Etc/UTC",
            //        End = new DateTime(2020, 4, 8),
            //        EndTimezone = "Etc/UTC",
            //        IsAllDay = true,
            //        RoomID = 3,
            //        Attendees = new List<int> {1, 2, 3}
            //    }
            //}.AsQueryable();

            //return Json(resultList.ToDataSourceResult(request));
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
