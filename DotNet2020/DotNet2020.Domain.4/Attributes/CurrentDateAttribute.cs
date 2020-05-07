using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Attributes
{
    public class LessThanCurrentDateAttribute : ValidationAttribute
    {
        public LessThanCurrentDateAttribute()
        {
        }
        
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            return date.Date < DateTime.Now.Date;
        }
    }
}
