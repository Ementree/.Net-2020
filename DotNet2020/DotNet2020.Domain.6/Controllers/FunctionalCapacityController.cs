using DotNet2020.Domain._6.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DotNet2020.Domain._6.Services;

namespace DotNet2020.Domain._6.Controllers
{
    public class FunctionalCapacityController : Controller
    {
        public readonly DbContext _context;

        public FunctionalCapacityController(DbContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index(int year = -1,int currentAccuracy = 5)       
        {
            //Предварительно стягивание с бд ResourceGroupType позволяет избежать .Include() в нескольких запросах
            var builder = new FunctionalCapacityViewModelBuilder(_context);
            var viewModel = builder.Build(year,currentAccuracy);

            return View(viewModel);
        }
    }
}