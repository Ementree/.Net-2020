using System;
using System.ComponentModel.DataAnnotations;
using DotNet2020.Domain._4.Attributes;
using DotNet2020.Domain.Attributes;

namespace DotNet2020.Domain._4_.Models.ModelView
{
    public class IllnessViewModel
    {
        [Required(ErrorMessage = "Введите дату")]
        public DateTime? From { get; set; }

        [Required(ErrorMessage = "Введите дату")]
        [GreaterThan("From", true)]
        [LessThanCurrentDate(ErrorMessage = "SickDay можно брать только на прошедние даты")]
        public DateTime? To { get; set; }
    }
}
