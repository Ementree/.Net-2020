using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._4.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly CalendarEntryContext _dbContext;

        public RecommendationController(CalendarEntryContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        #warning выделить в отдельный контроллер
        [HttpGet]
        [Authorize]
        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidationFilter]
        public IActionResult Update(Recommendation recommendation)
        {
            var dbEntry = _dbContext.Recommendations.FirstOrDefault();
            if (dbEntry == null)
                _dbContext.Recommendations.Add(recommendation);
            else dbEntry.RecommendationText = recommendation.RecommendationText;
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Admin", "Calendar");
        }
    }
}