using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain.Core.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
