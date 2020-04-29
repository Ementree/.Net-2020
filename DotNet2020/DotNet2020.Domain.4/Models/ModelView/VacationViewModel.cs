using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
