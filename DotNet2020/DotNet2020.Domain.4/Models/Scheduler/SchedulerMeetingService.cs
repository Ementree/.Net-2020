namespace Kendo.Mvc.Examples.Models.Scheduler
{
    using Kendo.Mvc.UI;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Collections;
    using Microsoft.AspNetCore.Http;
    using Extensions;

    public class SchedulerMeetingService : BaseService, ISchedulerEventService<MeetingViewModel>
    {
        private static bool UpdateDatabase = false;
        private ISession _session;

        public ISession Session { get { return _session; } }

        public SchedulerMeetingService(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public virtual IQueryable<MeetingViewModel> GetAll()
        {
            return GetAllMeetings().AsQueryable();
        }

        public virtual IList<MeetingViewModel> GetAllMeetings()
        {
            using (var db = GetContext())
            {
                var result = Session.GetObjectFromJson<IList<MeetingViewModel>>("SchedulerMeetings");

                if (result == null || UpdateDatabase)
                {
                    result = db.Meetings
                        .Include(model => model.MeetingAttendees)
                        .ToList()
                        .Select(meeting => new MeetingViewModel
                        {
                            MeetingID = meeting.MeetingID,
                            Title = meeting.Title,
                            Start = DateTime.SpecifyKind(meeting.Start, DateTimeKind.Utc),
                            End = DateTime.SpecifyKind(meeting.End, DateTimeKind.Utc),
                            StartTimezone = meeting.StartTimezone,
                            EndTimezone = meeting.EndTimezone,
                            Description = meeting.Description,
                            IsAllDay = meeting.IsAllDay,
                            RoomID = meeting.RoomID,
                            RecurrenceRule = meeting.RecurrenceRule,
                            RecurrenceException = meeting.RecurrenceException,
                            RecurrenceID = meeting.RecurrenceID,
                            Attendees = meeting.MeetingAttendees.Select(m => m.AttendeeID).ToArray()
                        }).ToList();

                    Session.SetObjectAsJson("SchedulerMeetings", result);
                }

                return result;
            }
        }

        public virtual void Insert(MeetingViewModel meeting, ModelStateDictionary modelState)
        {
            if (ValidateModel(meeting, modelState))
            {
                if (!UpdateDatabase)
                {
                    var meetings = GetAllMeetings();
                    var first = meetings.OrderByDescending(e => e.MeetingID).FirstOrDefault();
                    var id = (first != null) ? first.MeetingID : 0;

                    meeting.MeetingID = id + 1;

                    meetings.Insert(0, meeting);

                    Session.SetObjectAsJson("SchedulerMeetings", meetings);
                }
                else
                {
                    using (var db = GetContext())
                    {
                        if (meeting.Attendees == null)
                        {
                            meeting.Attendees = new int[0];
                        }

                        if (string.IsNullOrEmpty(meeting.Title))
                        {
                            meeting.Title = "";
                        }

                        var entity = meeting.ToEntity();

                        foreach (var attendeeId in meeting.Attendees)
                        {
                            entity.MeetingAttendees.Add(new MeetingAttendee
                            {
                                AttendeeID = attendeeId
                            });
                        }

                        db.Meetings.Add(entity);
                        db.SaveChanges();

                        meeting.MeetingID = entity.MeetingID;
                    }
                }
            }
        }

        public virtual void Update(MeetingViewModel meeting, ModelStateDictionary modelState)
        {
            if (ValidateModel(meeting, modelState))
            {
                if (!UpdateDatabase)
                {
                    var meetings = GetAllMeetings();
                    var target = meetings.FirstOrDefault(e => e.MeetingID == meeting.MeetingID);

                    if (target != null)
                    {
                        target.Title = meeting.Title;
                        target.Start = meeting.Start;
                        target.End = meeting.End;
                        target.StartTimezone = meeting.StartTimezone;
                        target.EndTimezone = meeting.EndTimezone;
                        target.Description = meeting.Description;
                        target.IsAllDay = meeting.IsAllDay;
                        target.RecurrenceRule = meeting.RecurrenceRule;
                        target.RoomID = meeting.RoomID;
                        target.RecurrenceException = meeting.RecurrenceException;
                        target.RecurrenceID = meeting.RecurrenceID;
                        target.Attendees = meeting.Attendees;
                    }

                    Session.SetObjectAsJson("SchedulerMeetings", meetings);
                }
                else
                {
                    using (var db = GetContext())
                    {
                        if (string.IsNullOrEmpty(meeting.Title))
                        {
                            meeting.Title = "";
                        }

                        var entity = meeting.ToEntity();
                        db.Meetings.Attach(entity);
                        db.Entry(entity).State = EntityState.Modified;

                        var oldMeeting = db.Meetings
                            .Include(model => model.MeetingAttendees)
                            .FirstOrDefault(m => m.MeetingID == meeting.MeetingID);

                        foreach (var attendee in oldMeeting.MeetingAttendees.ToList())
                        {
                            db.MeetingAttendees.Attach(attendee);

                            if (meeting.Attendees == null || !meeting.Attendees.Contains(attendee.AttendeeID))
                            {
                                db.Entry(attendee).State = EntityState.Deleted;
                            }
                            else
                            {
                                db.Entry(attendee).State = EntityState.Unchanged;

                                ((List<int>)meeting.Attendees).Remove(attendee.AttendeeID);
                            }

                            entity.MeetingAttendees.Add(attendee);
                        }

                        if (meeting.Attendees != null)
                        {
                            foreach (var attendeeId in meeting.Attendees)
                            {
                                var meetingAttendee = new MeetingAttendee
                                {
                                    MeetingID = entity.MeetingID,
                                    AttendeeID = attendeeId
                                };

                                db.MeetingAttendees.Attach(meetingAttendee);
                                db.Entry(meetingAttendee).State = EntityState.Added;

                                entity.MeetingAttendees.Add(meetingAttendee);
                            }
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        public virtual void Delete(MeetingViewModel meeting, ModelStateDictionary modelState)
        {
            if (!UpdateDatabase)
            {
                var meetings = GetAllMeetings();
                var target = meetings.FirstOrDefault(e => e.MeetingID == meeting.MeetingID);

                if (target != null)
                {
                    meetings.Remove(target);

                    var recurrenceExceptions = meetings.Where(m => m.RecurrenceID == meeting.MeetingID).ToList();

                    foreach (var recurrenceException in recurrenceExceptions)
                    {
                        meetings.Remove(recurrenceException);
                    }
                }
            }
            else
            {
                using (var db = GetContext())
                {
                    if (meeting.Attendees == null)
                    {
                        meeting.Attendees = new int[0];
                    }

                    var entity = meeting.ToEntity();

                    db.Meetings.Attach(entity);

                    var attendees = meeting.Attendees.Select(attendee => new MeetingAttendee
                    {
                        AttendeeID = attendee,
                        MeetingID = entity.MeetingID
                    });

                    foreach (var attendee in attendees)
                    {
                        db.MeetingAttendees.Attach(attendee);
                        db.Entry(attendee).State = EntityState.Deleted;
                    }

                    var recurrenceExceptions = db.Meetings.Where(m => m.RecurrenceID == entity.MeetingID);

                    foreach (var recurrenceException in recurrenceExceptions)
                    {
                        db.Meetings.Remove(recurrenceException);
                    }

                    db.Entry(entity).State = EntityState.Deleted;
                    db.Meetings.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        private bool ValidateModel(MeetingViewModel appointment, ModelStateDictionary modelState)
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