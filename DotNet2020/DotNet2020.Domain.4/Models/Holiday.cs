using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet2020.Domain._4.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Введите название праздника")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите дату праздника")]
        public DateTime Date { get; set; }
    }
}
