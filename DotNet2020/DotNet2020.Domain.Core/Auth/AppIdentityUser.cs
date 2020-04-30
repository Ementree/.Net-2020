using Microsoft.AspNetCore.Identity;

namespace DotNet2020.Data
{
    public class AppIdentityUser : IdentityUser
    {
        public string FirstName { get; protected set; }
        public string LastName { get;protected set; }
        public string Position { get;protected set; }
        public bool IsLastVacationApproved { get; protected set; }
        public string Test { get; protected set; }

        public int TotalDayOfVacation { get; set; }

        public AppIdentityUser(string userName, string email, string firstName, string lastName, string position, string test)
            : base(userName)
        {
            Email = email;
            //to do validate email!!!
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Test = test;
            TotalDayOfVacation = 28;
        }

        // For EF Core only
        public AppIdentityUser()
            : base()
        {
           
        }

        public void Approve()
        {
            IsLastVacationApproved = true;
        }

        public void Reject()
        {
            IsLastVacationApproved = false;
        }
    }
}
