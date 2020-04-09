using DotNet2020.Domain._6.Models;
using DotNet2020.Domain._6.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNet2020.Domain._6.Controllers
{
    public class AbsenceController : Controller
    {
        private DbContext context;
        public AbsenceController(DbContext context)
        {
            this.context = context;
        }
        // GET
        public IActionResult Index()
        {
            var viewAbsences = new Absences();

            var absences = new List<CalendarEntry>();

            for(int i = 0;i < 6; i++)
            {
                absences.Add(new CalendarEntry() { UserName = "User " + i.ToString() });
            }

            var periods = context.Set<Period>().ToList();
            viewAbsences.Periods = periods;

            var resourceAbsences = new Dictionary<string, List<int>>();

            foreach(var absence in absences)
            {
                var countAbs = new List<int>();
                foreach(var per in periods)
                {
                    countAbs.Add(0);
                }
                resourceAbsences.Add(absence.UserName, countAbs);
            }
            viewAbsences.ResourceAbsences = resourceAbsences;
            return View(viewAbsences);
        }
    }

    
}