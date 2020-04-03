namespace Kendo.Mvc.Examples.Models.Scheduler
{
    using System.Linq;
    using Kendo.Mvc.UI;
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using Extensions;

    public class SchedulerTaskService : BaseService, ISchedulerEventService<TaskViewModel>
    {
        private static bool UpdateDatabase = false;
        private ISession _session;

        public ISession Session { get { return _session; } }

        public SchedulerTaskService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public virtual IQueryable<TaskViewModel> GetAll()
        {
            return GetAllTasks().AsQueryable();
        }

        private IList<TaskViewModel> GetAllTasks()
        {
            using (var db = GetContext())
            {
                var result = Session.GetObjectFromJson<IList<TaskViewModel>>("SchedulerTasks");

                if (result == null || UpdateDatabase)
                {
                    result = db.Tasks.ToList().Select(task => new TaskViewModel
                    {
                        TaskID = task.TaskID,
                        Title = task.Title,
                        Start = DateTime.SpecifyKind(task.Start, DateTimeKind.Utc),
                        End = DateTime.SpecifyKind(task.End, DateTimeKind.Utc),
                        StartTimezone = task.StartTimezone,
                        EndTimezone = task.EndTimezone,
                        Description = task.Description,
                        IsAllDay = task.IsAllDay,
                        RecurrenceRule = task.RecurrenceRule,
                        RecurrenceException = task.RecurrenceException,
                        RecurrenceID = task.RecurrenceID,
                        OwnerID = task.OwnerID
                    }).ToList();

                    Session.SetObjectAsJson("SchedulerTasks", result);
                }

                return result;
            }
        }

        public virtual void Insert(TaskViewModel task, ModelStateDictionary modelState)
        {
            if (ValidateModel(task, modelState))
            {
                if (!UpdateDatabase)
                {
                    var tasks = GetAllTasks();
                    var first = tasks.OrderByDescending(e => e.TaskID).FirstOrDefault();

                    var id = (first != null) ? first.TaskID : 0;

                    task.TaskID = id + 1;

                    tasks.Insert(0, task);

                    Session.SetObjectAsJson("SchedulerTasks", tasks);
                }
                else
                {
                    using (var db = GetContext())
                    {
                        if (string.IsNullOrEmpty(task.Title))
                        {
                            task.Title = "";
                        }

                        var entity = task.ToEntity();

                        db.Tasks.Add(entity);
                        db.SaveChanges();

                        task.TaskID = entity.TaskID;
                    }
                }
            }
        }

        public virtual void Update(TaskViewModel task, ModelStateDictionary modelState)
        {
            if (ValidateModel(task, modelState))
            {
                if (!UpdateDatabase)
                {
                    var tasks = GetAllTasks();
                    var target = tasks.FirstOrDefault(e => e.TaskID == task.TaskID);

                    if (target != null)
                    {
                        target.Title = task.Title;
                        target.Start = task.Start;
                        target.End = task.End;
                        target.StartTimezone = task.StartTimezone;
                        target.EndTimezone = task.EndTimezone;
                        target.Description = task.Description;
                        target.IsAllDay = task.IsAllDay;
                        target.RecurrenceRule = task.RecurrenceRule;
                        target.RecurrenceException = task.RecurrenceException;
                        target.RecurrenceID = task.RecurrenceID;
                        target.OwnerID = task.OwnerID;
                    }

                    Session.SetObjectAsJson("SchedulerTasks", tasks);
                }
                else
                {
                    using (var db = GetContext())
                    {
                        if (string.IsNullOrEmpty(task.Title))
                        {
                            task.Title = "";
                        }

                        var entity = task.ToEntity();
                        db.Tasks.Attach(entity);
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        public virtual void Delete(TaskViewModel task, ModelStateDictionary modelState)
        {
            if (!UpdateDatabase)
            {
                var tasks = GetAllTasks();
                var target = tasks.FirstOrDefault(e => e.TaskID == task.TaskID);

                if (target != null)
                {
                    tasks.Remove(target);

                    var recurrenceExceptions = tasks.Where(m => m.RecurrenceID == task.TaskID).ToList();

                    foreach (var recurrenceException in recurrenceExceptions)
                    {
                        tasks.Remove(recurrenceException);
                    }
                }

                Session.SetObjectAsJson("SchedulerTasks", tasks);
            }
            else
            {
                using (var db = GetContext())
                {
                    var entity = task.ToEntity();
                    db.Tasks.Attach(entity);

                    var recurrenceExceptions = db.Tasks.Where(t => t.RecurrenceID == task.TaskID);

                    foreach (var recurrenceException in recurrenceExceptions)
                    {
                        db.Tasks.Remove(recurrenceException);
                    }

                    db.Tasks.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        //TODO: better naming or refactor
        private bool ValidateModel(TaskViewModel appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }
            
            return true;
        }
    }
}