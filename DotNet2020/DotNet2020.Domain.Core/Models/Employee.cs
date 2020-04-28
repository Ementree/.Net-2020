namespace DotNet2020.Domain.Core.Models
{
    public class Employee: HasIdBase
    { 
        public string FirstName { get; set; }

        public string LastName { get; set; }
       
        public string MiddleName { get; set; }
       
        public string Position { get; set; }
    }
}