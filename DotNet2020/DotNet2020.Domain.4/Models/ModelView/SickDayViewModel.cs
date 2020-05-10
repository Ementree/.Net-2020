using System;
using System.ComponentModel.DataAnnotations;
using DotNet2020.Domain.Attributes;

namespace DotNet2020.Domain._4_.Models.ModelView
{
    public class SickDayViewModel
    {
        [Required(ErrorMessage = "Введите дату")]
        [LessThanCurrentDate(ErrorMessage = "SickDay можно брать только на прошедние даты")]
        public DateTime? Day { get; set; }
    }
}
