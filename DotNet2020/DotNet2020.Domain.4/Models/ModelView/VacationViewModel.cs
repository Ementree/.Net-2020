using System;
using System.ComponentModel.DataAnnotations;
using DotNet2020.Domain._4.Attributes;
using DotNet2020.Domain.Attributes;

namespace DotNet2020.Domain._4_.Models.ModelView
{
    public class VacationViewModel
    {
        [Required(ErrorMessage = "Введите дату")]
        public DateTime? From { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [GreaterThan("From", true)]
        [MoreThanCurrentDate(ErrorMessage = "Отпуск можно брать только на предстоящие даты")]
        public DateTime? To { get; set; }

        [Required(ErrorMessage = "Выберите тип отпуска")]
        public bool IsPaid { get; set; }
    }
}
