using System.Linq;
using DotNet2020.Data;
using DotNet2020.Domain.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain.Filters
{
    public class EmployeeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dbContext = context.Controller.GetType().GetField("_dbContext").GetValue(context.Controller) as DbContext;
            var employee = dbContext.Set<AppIdentityUser>()
                    .Include(u => u.Employee)
                    .FirstOrDefault(u => context.HttpContext.User.Identity.Name == u.Email).Employee;
            if (employee == default)
            {
                //return RedirectToAction("WorkersAdd", "Attestation");
                context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Index" }
                });

                //context.RouteData.Values.Add("message", "Ваш аккаунт не привязан к Employee, вы не можете просматривать календарь");

                Controller controller = context.Controller as Controller;

                // IController is not necessarily a Controller
                if (controller != null)
                {
                    controller.TempData.Add("message", "Ваш аккаунт не привязан к Employee, вы не можете просматривать календарь");
                }
            }
        }
    }
}
