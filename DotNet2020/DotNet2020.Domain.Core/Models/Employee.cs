using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Core.Models
{
    public class Employee : HasIdBase
    {
        [Required, StringLength(Strings.DefaultLength)]
        public string FirstName { get; set; }

        [Required, StringLength(Strings.DefaultLength)]
        public string LastName { get; set; }

        [StringLength(Strings.DefaultLength)]
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }

        public Position Position { get; set; }
    }
}