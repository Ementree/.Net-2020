using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._4.Attributes
{
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly DateTime _date = new DateTime();
        public GreaterThanAttribute(DateTime date)
        {
            _date = date;
        }
        
        public override bool IsValid(object value)
        {
            var result = base.IsValid(value);
            if (value is DateTime date)
                result = (DateTime?) date >= _date;
            return result;
        }
    }
}