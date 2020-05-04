﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNet2020.Domain._3.Helpers;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using DotNet2020.Domain._3.Repository;
using DotNet2020.Domain._3.Repository.Main;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._3.Controllers
{
    public class AttestationController:Controller
    {
        private readonly AttestationRepository _attestation;
        private readonly SpecificWorkerRepository _workers;
        private readonly GradesRepository _grades;
        private readonly CompetencesRepository _competences;
        private readonly IWebHostEnvironment _env;
        public AttestationController(AttestationContext attestationContext, IWebHostEnvironment env)
        {
            _workers = new SpecificWorkerRepository(attestationContext);
            _attestation = new AttestationRepository(attestationContext);
            _grades=new GradesRepository(attestationContext);
            _competences=new CompetencesRepository(attestationContext);
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Workers()
        {
            ViewBag.Workers = WorkerOutputModelHelper.GetList(_workers);
            return View();
        }
        
        public IActionResult WorkersUpdate(long id)
        {
            ViewBag.Competences = _competences.GetList();
            ViewBag.Worker = _workers.GetById(id);
            ViewBag.Ids = _workers.GetAllCompetencesIdsById(id);
            return View();
        }
        
        [HttpPost]
        public IActionResult WorkersUpdate(long id, SpecificWorkerModel workerModel, List<long> ids)
        {
            _workers.Update(workerModel);
            _workers.UpdateTable(workerModel, ids);
            return RedirectToAction("Workers");
        }

        public IActionResult WorkersAdd()
        {
            ViewBag.Competences = _competences.GetList();
            return View();
        }
        
        [HttpPost]
        public IActionResult WorkersAdd(SpecificWorkerModel workerModel, List<long> competences)
        {
            ViewBag.Competences = _competences.GetList();
            _workers.AddToAnotherTable(workerModel, competences);
            _workers.Create(workerModel);
            return RedirectToAction("Workers");
        }
        
        public IActionResult WorkersRemove(long id)
        {
            _workers.DeleteById(id);
            return RedirectToAction("Workers");
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
            var competence= new CompetencesModel {Competence = competenceName, Content = new string[]{competenceName}, Questions = new string[]
            {
                "Вопрос №1",
                "Вопрос №2"
            }};
            _competences.Create(competence);
            return RedirectToAction("CompetencesManage", new { id = competence.Id });
        }
        
        public IActionResult CompetencesManage(long id)
        {
            ViewBag.Content = _competences.GetById(id).Content;
            return View();
        }
        
        [HttpPost]
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
            var a =GradeOutputModelHelper.GetList(_grades);
            ViewBag.Grades = GradeOutputModelHelper.GetList(_grades);
            return View();
        }
        
        public IActionResult GradesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GradesAdd(string gradeName)
        {
            var grade = new GradesModel {Grade = gradeName};
            _grades.Create(grade);
            return RedirectToAction("GradesManage", new { id = grade.Id });
        }
        
        public IActionResult GradesManage(long id)
        {
            ViewBag.CompetencesList =  _competences.GetList();
            ViewBag.AddedCompetences = _grades.GetAllCompetencesById(id);
            return View();
        }

        [HttpPost]
        public IActionResult GradesManage(long id, string method, List<long> competences)
        {
            GradeHelper.Manage(_grades, id, method, competences);
            if (method == "RemoveGrade")
                return RedirectToAction("Grades");
            ViewBag.CompetencesList =  _competences.GetList();
            ViewBag.AddedCompetences = _grades.GetAllCompetencesById(id);
            return View();
        }
        
        public IActionResult Questions()
        {
            ViewBag.Questions = _competences.GetList();
            return View();
        }
        
        public IActionResult QuestionsManage(long id)
        {
            ViewBag.Questions = _competences.GetById(id).Questions;
            return View();
        }
        
        [HttpPost]
        public IActionResult QuestionsManage(long id, string method, string question, List<int> checkboxes)
        {
            QuestionHelper.Manage(_competences, id, method, question, checkboxes);
            ViewBag.Questions = _competences.GetById(id).Questions;
            return View();
        }
        
        public IActionResult Attestation()
        {
            ViewBag.Method = "Choose";
            ViewBag.Type = "";
            ViewBag.Workers = WorkerOutputModelHelper.GetList(_workers);
            ViewBag.Competences = _competences.GetList();
            ViewBag.Grades = _grades.GetList();
            return View();
        }
        
        [HttpPost]
        public IActionResult Attestation(bool? isGotGrade,long? gradeId, string type,string method, AttestationModel model, List<long> rightAnswers, List<long> skipedAnswers, 
            List<string> commentaries, List<long> gotCompetences, List<string> questions)
        {
            ViewBag.Grades = _grades.GetList();
            ViewBag.Type = type;
            ViewBag.Workers = WorkerOutputModelHelper.GetList(_workers);
            ViewBag.Competences = _competences.GetList();
            if (type != ""&& method=="Choose")
            {
                ViewBag.Method = "Choose";
                ViewBag.GradeId = gradeId;
                ViewBag.Type = type;
                return View();
            }
            if (model.CompetencesId == null)
                model.CompetencesId = AttestationHelper.GetCompetencesForGrade(gradeId.Value, _grades);
            if (model.WorkerId != null && model.CompetencesId.Count > 0)
            {
                switch (method)
                {
                    case ("Attestation"):
                        ViewBag.GradeId = gradeId;
                        ViewBag.Questions =
                            AttestationHelper.GetQuestionsForCompetences(model.CompetencesId, _competences);
                        ViewBag.ChosenCompetences =
                            AttestationHelper.GetNamesOfChosenCompetences(model.CompetencesId, _competences);
                        ViewBag.WorkerId = model.WorkerId;
                        ViewBag.CompetencesId = model.CompetencesId;
                        break;
                    case ("Finished"):
                        model.TestedCompetences = model.CompetencesId.ToList();
                        if (isGotGrade != null && isGotGrade == false)
                        {
                            model.CompetencesId.Clear();
                        }

                        if (isGotGrade!=null && isGotGrade == true && gradeId!=null)
                            gotCompetences = AttestationHelper.GetCompetencesForGrade(gradeId.Value, _grades);

                        AttestationHelper.SaveAttestation(rightAnswers, skipedAnswers, commentaries, gotCompetences, questions, model, _workers, _attestation);
                        return RedirectToAction("AttestationList");
                }
                ViewBag.Method = method;
                ViewBag.Type = type;
            }
            else
                ViewBag.Method = "Choose";
            return View(model);
        }

        public IActionResult AttestationList()
        {
            List<OutputHelper> outputHelpers = new List<OutputHelper>();
            foreach (var element in _attestation.GetList())
            {
                outputHelpers.Add(new OutputHelper(_competences, _workers, element));
            }
            ViewBag.Attestations = outputHelpers;
            return View();
        }
        
        public IActionResult DownloadAttestation(long id)
        {
            PdfHelper.GetPdfOfAttestation(id, _attestation, _workers, _competences);
            var stream = new FileStream(Path.Combine(_env.ContentRootPath, "Files", "attestation.pdf"), FileMode.OpenOrCreate);
            return File(stream, "application/pdf", "attestation.pdf");
        }

        public IActionResult Output()
        {
            ViewBag.Workers = WorkerOutputModelHelper.GetList(_workers);
            return View();
        }
        
        [HttpPost]
        public IActionResult Output(List<long> ids)
        {
            ViewBag.Workers = WorkerOutputModelHelper.GetList(_workers);
            PdfHelper.GetPdfofWorkers(ids, _workers);
            var stream = new FileStream(Path.Combine(_env.ContentRootPath, "Files", "workers.pdf"), FileMode.OpenOrCreate);
            return File(stream, "application/pdf", "workers.pdf");
        }
    }
}