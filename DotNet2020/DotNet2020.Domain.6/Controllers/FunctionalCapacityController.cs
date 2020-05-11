using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Channels;
//using ASP;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DotNet2020.Domain._6.Services;

namespace DotNet2020.Domain._6.Controllers
{
    [Route("[controller]")]
    public class FunctionalCapacityController : Controller
    {
        public readonly DbContext _context;

        public FunctionalCapacityController(DbContext context)
        {
            _context = context;
        }

        [HttpGet("getAbsences")]
        public Dictionary<string, Dictionary<string, int>> GetAbsences(int year)
        {
            var hardcode = new Dictionary<string,Dictionary<string,int>>();
            
            hardcode["Артур Саттаров"] = new Dictionary<string, int>();
            hardcode["Артур Саттаров"]["Май"] = 8848;

            return hardcode;
        }

        // GET
        //int year = -1,int currentAccuracy = 5
        public IActionResult Index(FunctionalCapacityViewModelBuilderOptions options)       
        {
            var builder = new FunctionalCapacityViewModelBuilder(_context);
            var viewModel = builder.Build(options);

            return View(viewModel);
        }
    }
}