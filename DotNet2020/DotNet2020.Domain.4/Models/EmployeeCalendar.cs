using System;
using DotNet2020.Domain.Core.Models;

namespace DotNet2020.Domain.Models
{
    public class EmployeeCalendar
    {
        public int Id { get; set; }
        public Employee Employee { get; set; }

        public int TotalDayOfVacation { get; set; }
        public bool IsLastVacationApproved { get; private set; }

        public void Approve()
        {
            IsLastVacationApproved = true;
        }

        public void Reject()
        {
            IsLastVacationApproved = false;
        }

        public string UserName
        {
            get
            {
                return $"{Employee.FirstName} {Employee.LastName}";
            }
        }
    }
}
