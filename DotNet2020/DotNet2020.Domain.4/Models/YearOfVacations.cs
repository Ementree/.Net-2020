using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Models
{
    public class YearOfVacations
    {
        [Key]
        [Required]
        public int Year { get; set; }
    }
}
