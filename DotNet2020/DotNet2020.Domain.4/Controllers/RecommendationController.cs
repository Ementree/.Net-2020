using System.Linq;
using DotNet2020.Domain._4.Models;
using DotNet2020.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._4.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly DbContext _dbContext;

        public RecommendationController(DbContext dbContext)
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
            var dbEntry = _dbContext.Set<Recommendation>().FirstOrDefault();
            if (dbEntry == null)
                _dbContext.Set<Recommendation>().Add(recommendation);
            else dbEntry.RecommendationText = recommendation.RecommendationText;
            _dbContext.SaveChanges();
            return RedirectToActionPermanent("Index", "AdminCalendar");
        }
    }
}