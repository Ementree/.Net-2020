using System;
using System.ComponentModel.DataAnnotations;
using DotNet2020.Domain._4.Attributes;

namespace DotNet2020.Domain._4_.Models.ModelView
{
    public class VacationViewModel
    {
        [Required(ErrorMessage = "Введите дату")]
        public DateTime? From { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        public DateTime? To { get; set; }
    }
}
