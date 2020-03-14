﻿using System.Collections.Generic;
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
        private readonly GradesRepository _grades;
        private readonly QuestionsRepository _questions;
        public AttestationController(CompetencesContext competences, GradesContext grades, QuestionsContext questions)
        {
            _competences=new CompetencesRepository(competences);
            _grades=new GradesRepository(grades);
            _questions=new QuestionsRepository(questions);
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
            ViewBag.Grades = _grades.GetList();
            return View();
        }
        
        public IActionResult GradesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GradesAdd(string gradeName)
        {
            var grade = new GradesModels {Grade = gradeName, Competences = new string[2]{"Компетенция №1", "Компетенция №2"}};
            _grades.Create(grade);
            return RedirectToAction("GradesManage", new { id = grade.Id });
        }
        
        [Route("Attestation/GradesManage/{id}")]
        public IActionResult GradesManage(long id)
        {
            ViewBag.CompetencesList = _competences.GetList();
            ViewBag.Competences = _grades.GetById(id).Competences;
            return View();
        }

        [HttpPost]
        [Route("Attestation/GradesManage/{id}")]
        public IActionResult GradesManage(long id, string method, string competenceItem, List<int> checkboxes)
        {
            ViewBag.CompetencesList = _competences.GetList();
            GradeHelper.Manage(_grades, id, method, competenceItem, checkboxes);
            if (method == "RemoveGrade")
                return RedirectToAction("Grades");
            ViewBag.Competences = _grades.GetById(id).Competences;
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