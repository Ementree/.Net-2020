using System.Threading.Tasks;
using DotNet2020.Domain._4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CalendarController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddHoliday()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRecommendation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecommendation(Recommendation recommendation)
        {
            if (ModelState.IsValid)
            {
                var dbEntry = await _dbContext.Recommendations.FirstOrDefaultAsync();
                if (dbEntry == null)
                    await _dbContext.Recommendations.AddAsync(recommendation);
                else dbEntry.RecommendationText = recommendation.RecommendationText;
                await _dbContext.SaveChangesAsync();
                return RedirectToActionPermanent("Admin");
            }

            return RedirectToActionPermanent("AddRecommendation");
        }
    }
}
