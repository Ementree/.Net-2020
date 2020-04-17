using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNet2020.Domain._4_.Models.ModelView
{
    public class SickDayViewModel
    {
        [Required(ErrorMessage = "Введите дату")]
        public DateTime Day { get; set; }
    }
}
