using Kendo.Mvc.Examples.Models.Scheduler;
using Kendo.Mvc.Rendering;
using Kendo.Mvc.UI;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Kendo.Mvc.Examples.Models
{
    public class DemoServices
    {
        public static IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return ServiceDescriptor.Scoped<ISchedulerEventService<TaskViewModel>, SchedulerTaskService>();
            yield return ServiceDescriptor.Scoped<ISchedulerEventService<MeetingViewModel>, SchedulerMeetingService>();
        }
    }
}
