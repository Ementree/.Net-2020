using System;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Attributes
{
    public class MoreThanCurrentDateAttribute : ValidationAttribute
    {
        public MoreThanCurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            return date.Date >= DateTime.Now.Date;
        }
    }
}
