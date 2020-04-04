using System;
using System.Collections.Generic;
using DotNet2020.Domain._4.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain.Controllers
{
    public class SchedulerController : Controller
    {
        public SchedulerController()
        {
        }

        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var resultList = new List<ISchedulerEvent>()
            {
                new Vacation(DateTime.Now, new DateTime(2020, 4, 15), "Иванов Иван Иванович")
            };

            var result = Json(resultList.ToDataSourceResult(request));

            return result;
        }
    }
}
