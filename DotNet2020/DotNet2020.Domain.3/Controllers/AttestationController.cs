using System.Collections.Generic;
using DotNet2020.Domain._3.Helpers;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._3.Controllers
{
    public class AttestationController:Controller
    {
        private readonly CompetencesRepository _competences;
        public AttestationController(CompetencesContext context)
        {
            _competences=new CompetencesRepository(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Workers()
        {
            return View();
        }

        public IActionResult WorkersUpdate()
        {
            return View();
        }

        public IActionResult WorkersAdd()
        {
            return View();
        }

        public IActionResult Competences()
        {
            ViewBag.Competences = _competences.GetList();
            return View();
        }
        
        public IActionResult CompetencesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CompetencesAdd(string competenceName)
        {
            var competence= new CompetencesModel {Competence = competenceName, Content = new string[1]{competenceName}};
            _competences.Create(competence);
            return RedirectToAction("CompetencesManage", new { id = competence.Id });
        }
        
        [Route("Attestation/CompetencesManage/{id}")]
        public IActionResult CompetencesManage(long id)
        {
            ViewBag.Content = _competences.GetById(id).Content;
            return View();
        }
        
        [HttpPost]
        [Route("Attestation/CompetencesManage/{id}")]
        public IActionResult CompetencesManage(long id, string method, string contentItem, List<int> checkboxes)
        {
            CompetenceHelper.Manage(_competences, id, method, contentItem, checkboxes);
            if (method == "RemoveCompetence")
                return RedirectToAction("Competences");
            ViewBag.Content = _competences.GetById(id).Content;
            return View();
        }
        
        public IActionResult Grades()
        {
            return View();
        }
        
        public IActionResult GradesAdd()
        {
            return View();
        }
        
        public IActionResult GradesManage()
        {
            return View();
        }
        
        public IActionResult Questions()
        {
            return View();
        }
        
        public IActionResult QuestionsManage()
        {
            return View();
        }
        
        public IActionResult Attestation()
        {
            return View();
        }
        
        public IActionResult AttestationResults()
        {
            return View();
        }
        
        public IActionResult Output()
        {
            return View();
        }
    }
}