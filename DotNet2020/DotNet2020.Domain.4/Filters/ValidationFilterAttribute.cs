using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNet2020.Domain.Filters
{
    public enum ValidationResult
    {
        View,
        Json
    }

    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly ValidationResult _result;

        public ValidationFilterAttribute(ValidationResult result
            = ValidationResult.Json)
        {
            _result = result;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (_result == ValidationResult.Json)
                {
                    context.Result = new ValidationFailedResult(context.ModelState);
                }
                else
                {
                    context.Result = ((Controller)context.Controller).View(
                        context.ActionArguments.Values.First());
                    ValidationFailedResult.SetStatusCodeAndHeaders(
                        context.HttpContext);
                }
            }
        }
    }
}
