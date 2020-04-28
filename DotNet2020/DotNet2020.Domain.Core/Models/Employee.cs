using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Core.Models
{
    public class Employee: HasIdBase
    { 
        [Required, StringLength(Strings.DefaultLength)]
        public string FirstName { get; set; }

        [Required, StringLength(Strings.DefaultLength)]
        public string LastName { get; set; }
       
        [StringLength(Strings.DefaultLength)]
        public string MiddleName { get; set; }
       
        public Position Position { get; set; }
    }
}