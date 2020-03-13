using Microsoft.AspNetCore.Identity;

namespace DotNet2020.Domain.Core.Auth
{
    public class AppIdentityUser : IdentityUser
    {
        public string FirstName { get; protected set; }
        public string LastName { get;protected set; }
        public string Position { get;protected set; }

        public AppIdentityUser(string userName,string email,string firstName,string lastName,string position)
            :base(userName)
        {
            Email = email;
            //to do validate email!!!
            FirstName = firstName;
            LastName = lastName;
            Position = position;
        }

        // For EF Core only
        protected AppIdentityUser()
            : base()
        {
           
        }
    }
}
