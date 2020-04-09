using DotNet2020.Domain._4.Models;
using DotNet2020.Domain._6.Models;
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
            var absences = new List<AbstractCalendarEntry>();
            for(int i = 0;i < 6; i++)
            {
                absences.Add(new AbstractCalendarEntry() { UserName = "User " + i.ToString() });
            }
            var periods = context.Set<Period>().ToList();
            viewAbsences.Periods = periods;
            var dic = new Dictionary<string, List<int>>();
            foreach(var item in absences)
            {
                var a = new List<int>();
                foreach(var per in periods)
                {
                    a.Add(0);
                }
                dic.Add(item.UserName, a);
            }
            viewAbsences.ResourceAbsences = dic;
            return View(viewAbsences);
        }
    }

    public class Absences
    {
        public List<Period> Periods { get; set; }

        public Dictionary<string, List<int>> ResourceAbsences { get; set; }
    }
}