﻿using System;
using DotNet2020.Data;
using DotNet2020.Domain.Models;
using Kendo.Mvc.UI;

namespace DotNet2020.Domain._4.Models
{
    public abstract class AbstractCalendarEntry
    {
        public int Id { get; set; }
        public DateTime From { get; protected set; }
        public DateTime To { get; protected set; }
        public AbsenceType AbsenceType { get; set; }
        public int UserId { get; set; }
        public EmployeeCalendar User { get; set; }

        protected AbstractCalendarEntry(DateTime from, DateTime to, EmployeeCalendar user, AbsenceType type)
        {
            From = from;
            To = to;
            AbsenceType = type;
            User = user;
        }
        
        protected AbstractCalendarEntry(){}

        #warning Если есть этот метод, то From и To нужно сделать приватные setter'ы
        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException("You are trying set incorrect data period!" +
                "'From' parametr should be less than 'To' or equal to them");
            From = from;
            To = to;
        }

        public string UserName
        {
            get
            {
                if (User != null)
                    return $"{User.Employee.FirstName} {User.Employee.LastName}";
                else return "";
            }
        }
    }
}
