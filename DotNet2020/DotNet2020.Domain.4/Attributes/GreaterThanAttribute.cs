using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DotNet2020.Domain._4.Attributes
{
    //public sealed class GreaterThanAttribute : ValidationAttribute
    //{
    //    public string DateStartProperty { get; set; }
    //    public override bool IsValid(object value)
    //    {
    //        // Get Value of the DateStart property
    //        string dateStartString = HttpContext.Current.Request[DateStartProperty];
    //        DateTime dateEnd = (DateTime)value;
    //        DateTime dateStart = DateTime.Parse(dateStartString);

    //        // Meeting start time must be before the end time
    //        return dateStart < dateEnd;
    //    }
    //}

    public sealed class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;
        private readonly bool allowEqualDates;

        public GreaterThanAttribute(string testedPropertyName, bool allowEqualDates = false)
        {
            this.testedPropertyName = testedPropertyName;
            this.allowEqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("Неправильная дата", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)propertyTestedValue)
            {
                if (this.allowEqualDates && value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value > (DateTime)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Дата начала больничного должна быть меньше даты конца");
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule
        //    {
        //        ErrorMessage = this.ErrorMessageString,
        //        ValidationType = "isdateafter"
        //    };
        //    rule.ValidationParameters["propertytested"] = this.testedPropertyName;
        //    rule.ValidationParameters["allowequaldates"] = this.allowEqualDates;
        //    yield return rule;
        //}
    }
}