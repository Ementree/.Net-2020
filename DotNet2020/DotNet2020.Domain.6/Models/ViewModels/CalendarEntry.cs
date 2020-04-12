using System;

namespace DotNet2020.Domain._6.Models.ViewModels
{
    public class CalendarEntry// Пока нет общей бд использую этот класс, зависимость от другой команды, их класс абстракстный
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string UserName { get; set; }

        public void ChangeDate(DateTime from, DateTime to)
        {
            if (to < from) throw new ArgumentException("You are trying set incorrect data period!" +
                "'From' parametr should be less than 'To' or equal to them");
            From = from;
            To = to;
        }
    }
}
