using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Controllers
{
    public class PlanController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
