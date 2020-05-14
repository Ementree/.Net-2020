using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNet2020.Domain._3.Helpers;
using DotNet2020.Domain._3.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using DotNet2020.Data;
using Microsoft.EntityFrameworkCore;
using DotNet2020.Domain.Core.Models;
using DotNet2020.Domain._3.ViewModels;
using DotNet2020.Domain._3.Services;
using Microsoft.AspNetCore.Authorization;

namespace DotNet2020.Domain._3.Controllers
{
    [Authorize]
    public class AttestationController : Controller
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly WorkerService _workerService;
        private readonly GradeService _gradeService;
        private readonly QuestionService _questionService;
        private readonly AttestationService _attestationService;
        private readonly AnswerService _answerService;
        public AttestationController(DbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            _workerService = new WorkerService(context);
            _gradeService = new GradeService(context);
            _questionService = new QuestionService(context);
            _attestationService = new AttestationService(context);
            _answerService = new AnswerService(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Workers

        public IActionResult Workers()
        {
            var workers = _workerService.GetLoadedWorkers();
            return View(workers);
        }

        [Authorize(Roles = "admin")]
        public IActionResult WorkersUpdate(int id)
        {
            var worker = _workerService.GetWorker(id);
            var competences = _context.Set<CompetencesModel>();
            WorkerUpdateModel workerUpdateModel = new WorkerUpdateModel { Worker = worker, Competences = competences.ToList() };
            return View(workerUpdateModel);
        }

        [HttpPost]
        public IActionResult WorkersUpdate(int id, WorkerUpdateModel workerUpdateModel)
        {
            workerUpdateModel.Worker.Id = id; 

            var specificWorkerCompetences = _context.Set<SpecificWorkerCompetencesModel>();
            var competences = _context.Set<CompetencesModel>();
            var oldSpecificWorkerCompetencesModels = specificWorkerCompetences.Where(x => x.WorkerId == id);
            specificWorkerCompetences.RemoveRange(oldSpecificWorkerCompetencesModels); 

            foreach (var competenceId in workerUpdateModel.NewCompetencesIds) 
            {
                var competence = competences.Find(competenceId);
                var workerCompetences = new SpecificWorkerCompetencesModel 
                {
                    Competence = competence,
                    Worker = workerUpdateModel.Worker
                };
                workerUpdateModel.Worker.SpecificWorkerCompetencesModels.Add(workerCompetences);
                specificWorkerCompetences.Add(workerCompetences);
            }

            _context.Entry(workerUpdateModel.Worker.Position).State = EntityState.Modified; 
            _context.Entry(workerUpdateModel.Worker).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }

        public IActionResult WorkersAdd()
        {
            ViewBag.IsRegistering = false;
            ViewBag.Competences = _context.Set<CompetencesModel>().ToList();

            var employee = _workerService.TryGetEmployeeWithUsername(User.Identity.Name);
            if (employee == default)
                ViewBag.IsRegistering = true;
            return View();
        }

        [HttpPost]
        public IActionResult WorkersAdd(SpecificWorkerModel workerModel, List<long> competences)
        {
            ViewBag.Competences = _context.Set<CompetencesModel>().ToList(); 
            foreach (var competenceId in competences) 
            {
                var competence = _context.Set<CompetencesModel>().Find(competenceId);
                var workerCompetences = new SpecificWorkerCompetencesModel
                {
                    Competence = competence,
                    Worker = workerModel
                };
                workerModel.SpecificWorkerCompetencesModels.Add(workerCompetences);
            }

            var employee = _workerService.TryGetEmployeeWithUsername(User.Identity.Name);

            if (employee == default)
                workerModel.Email = User.Identity.Name;

            _context.Set<SpecificWorkerModel>().Add(workerModel);

            var user = _context.Set<AppIdentityUser>().FirstOrDefault(u => u.Email == workerModel.Email);

            if (user != default)
                user.Employee = workerModel;

            _context.SaveChanges();

            return RedirectToAction("Workers");
        }

        [Authorize(Roles = "admin")]
        public IActionResult WorkersRemove(int id)
        {
            var worker = _context.Set<SpecificWorkerModel>().Find(id);
           
            if (worker != null)
            {
                _context.Set<AppIdentityUser>().Where(x => x.Email == worker.Email).FirstOrDefault().Employee = null;
                _context.Set<SpecificWorkerModel>().Remove(worker);
            }
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }
        #endregion
        #region Competences

        [Authorize(Roles = "admin")]
        public IActionResult Competences()
        {
            var competences = _context.Set<CompetencesModel>();
            return View(competences);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CompetencesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CompetencesAdd(CompetencesModel competenceModel)
        {
            _context.Set<CompetencesModel>().Add(competenceModel);

            _context.SaveChanges();
            return RedirectToAction("CompetencesManage", new { id = competenceModel.Id });
        }

        [Authorize(Roles = "admin")]
        public IActionResult CompetencesManage(long id)
        {
            var competence = _context.Set<CompetencesModel>().Find(id);
            var competenceUpdateModel = new CompetenceUpdateModel { Competence = competence };
            return View(competenceUpdateModel);
        }

        [HttpPost]
        public IActionResult CompetencesManage(long id, CompetenceUpdateModel competenceUpdateModel)
        {
            var competence = _context.Set<CompetencesModel>().Find(id);
            switch (competenceUpdateModel.Action)
            {
                case CompetenceActions.AddContent:
                    competence.Content.Add(competenceUpdateModel.Content);
                    break;

                case CompetenceActions.RemoveCompetence:
                    _context.Set<CompetencesModel>().Remove(competence);
                    _context.SaveChanges();
                    return RedirectToAction("Competences");

                case CompetenceActions.RemoveContent:
                    List<string> newContent = new List<string>();
                    for (int i = 0; i < competence.Content.Count; i++)
                    {
                        if (!competenceUpdateModel.Checkboxes.Contains(i))
                            newContent.Add(competence.Content[i]);
                    }
                    competence.Content = newContent;
                    break;
            }

            competenceUpdateModel.Competence = competence;
            _context.SaveChanges();
            return View(competenceUpdateModel);
        }

        #endregion
        #region Grades

        [Authorize(Roles = "admin")]
        public IActionResult Grades()
        {
            var grades = _gradeService.GetLoadedGrades();
            return View(grades);
        }

        [Authorize(Roles = "admin")]
        public IActionResult GradesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GradesAdd(GradesModel gradeModel)
        {
            _context.Set<GradesModel>().Add(gradeModel);
            _context.SaveChanges();
            return RedirectToAction("GradesManage", new { id = gradeModel.Id });
        }

        [Authorize(Roles = "admin")]
        public IActionResult GradesManage(long id)
        {
            var gradeUpdateModel = _gradeService.GetGradeUpdateModelFor(id);
            ViewBag.UpdatedGrade = gradeUpdateModel.GradeModel;
            return View(gradeUpdateModel);
        }

        [HttpPost]
        public IActionResult GradesManage(long id, GradeUpdateModel gradeUpdateModel)
        {
            switch (gradeUpdateModel.Action)
            {
                case GradeActions.SetCompetences:
                    _gradeService.UpdateCompetencesForGrade(gradeUpdateModel, id);
                    break;
                case GradeActions.RemoveGrade:
                    _gradeService.RemoveGrade(id);
                    return RedirectToAction("Grades");
                case GradeActions.AddGrade:
                    _gradeService.AddNextGrade(gradeUpdateModel, id);
                    break;
            }

            gradeUpdateModel = _gradeService.GetGradeUpdateModelFor(id);
            ViewBag.UpdatedGrade = gradeUpdateModel.GradeModel;
            return View(gradeUpdateModel);
        }

        #endregion
        #region Questions
        [Authorize(Roles = "admin")]
        public IActionResult Questions()
        {
            var competenceQuestionsViewModels = _questionService.GetQuestions();
            return View(competenceQuestionsViewModels);
        }

        [Authorize(Roles = "admin")]
        public IActionResult QuestionsManage(long id)
        {
            var questionUpdateModel = _questionService.GetQuestionUpdateModel(id);
            return View(questionUpdateModel);
        }

        [HttpPost]
        public IActionResult QuestionsManage(long id, QuestionUpdateModel questionUpdateModel)
        {
            switch (questionUpdateModel.Action)
            {
                case QuestionActions.RemoveQuestions:
                    _questionService.RemoveQuestions(id, questionUpdateModel);
                    break;
                case QuestionActions.AddQuestion:
                    _questionService.AddNewQuestion(id, questionUpdateModel);
                    break;
            }

            questionUpdateModel = _questionService.GetQuestionUpdateModel(id);
            return View(questionUpdateModel);
        }

        #endregion
        #region Attestation
        [Authorize(Roles = "admin")]
        public IActionResult Attestation()
        {
            AttestationModel attestation = new AttestationModel();

            attestation.Workers = _workerService.GetLoadedWorkers();
            attestation.Grades = _gradeService.GetLoadedGrades();
            attestation.Competences = _context.Set<CompetencesModel>().ToList();

            if (attestation.Competences.Count == 0)
                attestation.Action = AttestationAction.None;
            else
            {
                if (attestation.Grades.Count != 0)
                    attestation.Action = AttestationAction.Choosing;
                else
                    attestation.Action = AttestationAction.CompetencesChose;
            }

            return View(attestation);
        }

        [HttpPost]
        public IActionResult Attestation(AttestationModel attestation)
        {
            switch (attestation.Action)
            {
                case AttestationAction.Choosing:
                    attestation.Workers = _workerService.GetLoadedWorkers();
                    break;

                case AttestationAction.CompetencesChose: //вывести таблицу компетенций 
                    attestation = _attestationService.CreateCompetenceTable(attestation);
                    break;

                case AttestationAction.GradeChose: //вывести таблицу грейдов
                    attestation = _attestationService.CreateGradeTable(attestation);
                    break;

                case AttestationAction.AttestationByCompetences: //вывести окно аттестации по компетенциям
                    attestation = _attestationService.CreateAttestationByCompetences(attestation);
                    break;

                case AttestationAction.AttestationByGrade: //вывести окно аттестации по грейдам
                    attestation = _attestationService.CreateAttestationByGrades(attestation, out var isReattistation);
                    attestation.ReAttestation = isReattistation;
                    ViewBag.ReAttestation = isReattistation;
                    break;

                case AttestationAction.Finished: //сохранить результаты
                    _attestationService.FinishAttestation(attestation);
                    return RedirectToAction("AttestationList");

                case AttestationAction.NotEnoughQuestion:
                    return View(attestation);
            }
            return View(attestation);
        }
        #endregion

        [Authorize(Roles = "admin")]
        public IActionResult AttestationList()
        {
            List<AttestationListModel> attestationListModels = new List<AttestationListModel>();
            foreach (var element in _context.Set<AttestationModel>().ToList())
            {
                var attestationListModel = new AttestationListModel(element);
                attestationListModel.Worker = _context.Set<SpecificWorkerModel>().Find((int)element.WorkerId);
                attestationListModel.Competences = element.GotCompetences.Select(x => _context.Set<CompetencesModel>().Find(x)).ToList();
                attestationListModels.Add(attestationListModel);
            }
            return View(attestationListModels);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Output()
        {
            var workers = _workerService.GetLoadedWorkers();
            return View(workers);
        }

        [HttpPost]
        public IActionResult Output(List<long> ids)
        {
            var stream = PdfHelper.GetPdfofWorkers(ids, _workerService.GetLoadedWorkers());
            return File(stream, "application/pdf", "workers.pdf");
        }

        [Authorize(Roles = "admin")]
        public IActionResult DownloadAttestation(long id)
        {
            PdfHelper.GetPdfOfAttestation(id, _context);
            var stream = PdfHelper.GetPdfOfAttestation(id, _context);
            return File(stream, "application/pdf", "attestation.pdf");
        }
    }
}