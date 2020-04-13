using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNet2020.Domain._3.Helpers;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Models.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DotNet2020.Domain._3.Controllers
{
    public class AttestationController : Controller
    {
        private readonly AttestationContext _context;
        private readonly IWebHostEnvironment _env;
        public AttestationController(AttestationContext attestationContext, IWebHostEnvironment env)
        {
            _context = attestationContext;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Workers

        public IActionResult Workers()
        {
            var workers = GetLoadedWorkers();

            return View(workers);
        }

        public IActionResult WorkersUpdate(long id)
        {
            var worker = _context.Workers.Find(id);

            _context.Entry(worker).Collection(x => x.SpecificWorkerCompetencesModels).Load();

            foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
            {
                specificWorkerCompetence.Competence = _context.Competences.Find(specificWorkerCompetence.CompetenceId);
            }

            WorkerUpdateModel workerUpdateModel = new WorkerUpdateModel { Worker = worker, Competences = _context.Competences.ToList() };

            return View(workerUpdateModel);
        }

        [HttpPost]
        public IActionResult WorkersUpdate(long id, WorkerUpdateModel workerUpdateModel)
        {
            workerUpdateModel.Worker.Id = id;

            var oldSpecificWorkerCompetencesModels = _context.SpecificWorkerCompetences.Where(x => x.WorkerId == id);
            _context.SpecificWorkerCompetences.RemoveRange(oldSpecificWorkerCompetencesModels);

            foreach (var competenceId in workerUpdateModel.NewCompetencesIds)
            {
                var competence = _context.Competences.Find(competenceId);

                var workerCompetences = new SpecificWorkerCompetencesModel();

                workerCompetences.Competence = competence;
                workerCompetences.Worker = workerUpdateModel.Worker;

                workerUpdateModel.Worker.SpecificWorkerCompetencesModels.Add(workerCompetences);
                _context.SpecificWorkerCompetences.Add(workerCompetences);
            }

            _context.Entry(workerUpdateModel.Worker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }

        public IActionResult WorkersAdd()
        {
            ViewBag.Competences = _context.Competences; //для вывода всех компетенций
            return View();
        }

        [HttpPost]
        public IActionResult WorkersAdd(SpecificWorkerModel workerModel, List<long> competences)
        {
            ViewBag.Competences = _context.Competences; //для вывода всех компетенций

            foreach (var competenceId in competences)
            {
                var competence = _context.Competences.Find(competenceId);

                var workerCompetences = new SpecificWorkerCompetencesModel();

                workerCompetences.Competence = competence;
                workerCompetences.Worker = workerModel;

                workerModel.SpecificWorkerCompetencesModels.Add(workerCompetences);
            }

            _context.Workers.Add(workerModel);
            _context.SaveChanges();

            return RedirectToAction("Workers");
        }

        public IActionResult WorkersRemove(long id)
        {
            var item = _context.Workers.Find(id);
            if (item != null)
                _context.Workers.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }
        #endregion
        #region Competences

        public IActionResult Competences()
        {
            var competences = _context.Competences;
            return View(competences);
        }

        public IActionResult CompetencesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CompetencesAdd(CompetencesModel competenceModel)
        {
            _context.Competences.Add(competenceModel);
            _context.SaveChanges();
            return RedirectToAction("CompetencesManage", new { id = competenceModel.Id });
        }

        public IActionResult CompetencesManage(long id)
        {
            var competence = _context.Competences.Find(id);
            var competenceUpdateModel = new CompetenceUpdateModel { Competence = competence };
            return View(competenceUpdateModel);
        }

        [HttpPost]
        public IActionResult CompetencesManage(long id, CompetenceUpdateModel competenceUpdateModel)
        {
            var competence = _context.Competences.Find(id);
            switch (competenceUpdateModel.Action)
            {
                case Models.CompetenceActions.AddContent:
                    competence.Content.Add(competenceUpdateModel.Content);
                    break;

                case Models.CompetenceActions.RemoveCompetence:
                    _context.Competences.Remove(competence);
                    _context.SaveChanges();
                    return RedirectToAction("Competences");

                case Models.CompetenceActions.RemoveContent:
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


        public IActionResult Grades()
        {
            var grades = _context.Grades.ToList();

            foreach (var grade in grades)
            {
                _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
                foreach (var gradeCompetences in grade.GradesCompetences)
                {
                    gradeCompetences.Competence = _context.Competences.Find(gradeCompetences.CompetenceId);
                }
            }

            return View(grades);
        }

        public IActionResult GradesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GradesAdd(GradesModel gradeModel)
        {
            _context.Grades.Add(gradeModel);
            _context.SaveChanges();
            return RedirectToAction("GradesManage", new { id = gradeModel.Id });
        }

        public IActionResult GradesManage(long id)
        {
            var grade = _context.Grades.Find(id);
            _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
            var gradeUpdateModel = new GradeUpdateModel { GradeModel = grade, Competences = _context.Competences.ToList() };
            return View(gradeUpdateModel);
        }

        [HttpPost]
        public IActionResult GradesManage(long id, GradeUpdateModel gradeUpdateModel)
        {
            var grade = _context.Grades.Find(id);
            switch (gradeUpdateModel.Action)
            {
                case GradeActions.SetCompetences:
                    var oldGradesCompetences = _context.GradeCompetences.Where(x => x.GradeId == id);
                    _context.GradeCompetences.RemoveRange(oldGradesCompetences);

                    foreach (var competenceId in gradeUpdateModel.NewCompetencesIds)
                    {
                        var gradeCompetence = new GradeCompetencesModel();

                        gradeCompetence.Competence = _context.Competences.Find(competenceId);
                        gradeCompetence.Grade = grade;

                        grade.GradesCompetences.Add(gradeCompetence);

                        _context.GradeCompetences.Add(gradeCompetence);
                    }
                    _context.SaveChanges();

                    break;
                case GradeActions.RemoveGrade:
                    _context.Grades.Remove(grade);
                    _context.SaveChanges();
                    return RedirectToAction("Grades");
            }

            _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
            gradeUpdateModel.GradeModel = grade;
            gradeUpdateModel.Competences = _context.Competences.ToList();
            return View(gradeUpdateModel);
        }

        #endregion
        #region Questions
        public IActionResult Questions()
        {
            var competences = _context.Competences;
            return View(competences);
        }

        public IActionResult QuestionsManage(long id)
        {
            var questions = _context.Competences.Find(id).Questions;
            var questionUpdateModel = new QuestionUpdateModel { Questions = questions };
            return View(questionUpdateModel);
        }

        [HttpPost]
        public IActionResult QuestionsManage(long id, QuestionUpdateModel questionUpdateModel)
        {
            var competence = _context.Competences.Find(id);
            switch (questionUpdateModel.Action)
            {
                case QuestionActions.RemoveQuestions:
                    foreach (var question in questionUpdateModel.QuestionsToRemove)
                    {
                        competence.Questions.Remove(question);
                    }
                    break;
                case QuestionActions.AddQuestion:
                    competence.Questions.Add(questionUpdateModel.NewQuestion);
                    break;
            }
            _context.SaveChanges();
            questionUpdateModel.Questions = competence.Questions;
            return View(questionUpdateModel);
        }

        #endregion
        #region Attestation
        public IActionResult Attestation()
        {
            AttestationModel attestation = new AttestationModel();

            attestation.Workers = GetLoadedWorkers();
            attestation.Grades = GetLoadedGrades();
            attestation.Competences = _context.Competences.ToList();

            if (attestation.Competences.Count == 0)
            {
                attestation.Action = AttestationAction.None;
            }
            else
            {
                if (attestation.Grades.Count != 0)
                {
                    attestation.Action = AttestationAction.Choosing;
                }
                else
                {
                    attestation.Action = AttestationAction.CompetencesChose;
                }
            }

            return View(attestation);
        }

        [HttpPost]
        public IActionResult Attestation(AttestationModel model)
        {
            switch (model.Action)
            {
                case AttestationAction.Choosing: //вывести окно выбора
                    break;

                case AttestationAction.CompetencesChose: //вывести таблицу компетенций и работников
                    model.Workers = GetLoadedWorkers();
                    model.Competences = _context.Competences.ToList();
                    model.Grades = GetLoadedGrades();
                    break;

                case AttestationAction.GradeChose: //вывести таблицу грейдов и работников
                    model.Workers = GetLoadedWorkers();
                    model.Competences = _context.Competences.ToList();
                    model.Grades = GetLoadedGrades();
                    break;

                case AttestationAction.AttestationByCompetences: //вывести окно аттестации по компетенциям
                    var questions = new List<string>();
                    var testedCompetences = new List<CompetencesModel>();
                    foreach (var competenceId in model.IdsTestedCompetences)
                    {
                        var competence = _context.Competences.Find(competenceId);
                        testedCompetences.Add(competence);
                        questions = questions.Union(competence.Questions).ToList();
                    }
                    questions = questions.Distinct().ToList();
                    model.Questions = questions;
                    model.TestedCompetences = testedCompetences;
                    break;

                case AttestationAction.AttestationByGrade: //вывести окно аттестации по грейдам
                    var questionsForGrade = new List<string>();
                    var gradeId = model.GradeId.Value;

                    var testedGradeCompetences = _context.GradeCompetences.Where(x => x.GradeId == gradeId).ToList();

                    var idsOfTestedCompetences = new List<CompetencesModel>();

                    foreach (var testedGradeCompetence in testedGradeCompetences)
                    {
                        var competence = _context.Competences.Find(testedGradeCompetence.CompetenceId);
                        questionsForGrade = questionsForGrade.Union(competence.Questions).ToList();
                        idsOfTestedCompetences.Add(competence);
                    }
                    questionsForGrade = questionsForGrade.Distinct().ToList();
                    model.TestedCompetences = idsOfTestedCompetences;
                    model.Questions = questionsForGrade;
                    break;

                case AttestationAction.Finished: //сохранить результаты
                    var tuple = GetProblemsAndAnswers(model);
                    model.Problems = tuple.Item1;
                    var answers = tuple.Item2;
                    model.Date = DateTime.Today;

                    if (model.GradeId != null) //по грейду
                    {
                        if (model.IsGotGrade != null)
                            model.GotCompetences = model.IdsTestedCompetences;
                        else
                            model.GotCompetences = new List<long>();
                    }

                    var gotCompetences = new List<long>();
                    var newCompetences = GetNewCompetences(model);

                    foreach (var newCompetence in newCompetences)
                    {
                        _context.SpecificWorkerCompetences.Add(newCompetence);
                        gotCompetences.Add(newCompetence.CompetenceId);
                    }

                    model.GotCompetences = gotCompetences;

                    AddAnswers(answers, model);

                    _context.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction("AttestationList");
            }
            return View(model);
        }
        #endregion
        private List<SpecificWorkerModel> GetLoadedWorkers()
        {
            var workers = _context.Workers.ToList();
            foreach (var worker in workers)
            {
                _context.Entry(worker).Collection(x => x.SpecificWorkerCompetencesModels).Load();
                foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
                {
                    specificWorkerCompetence.Competence = _context.Competences.Find(specificWorkerCompetence.CompetenceId);
                }
            }
            return workers;
        }

        private List<GradesModel> GetLoadedGrades()
        {
            var grades = _context.Grades.ToList();
            foreach (var grade in grades)
            {
                _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
                foreach (var gradeCompetences in grade.GradesCompetences)
                {
                    gradeCompetences.Competence = _context.Competences.Find(gradeCompetences.CompetenceId);
                }
            }
            return grades;
        }

        public IActionResult AttestationList()
        {
            List<AttestationListModel> attestationListModels = new List<AttestationListModel>();
            foreach (var element in _context.Attestations.ToList())
            {
                var attestationListModel = new AttestationListModel(element);
                attestationListModel.Worker = _context.Workers.Find(element.WorkerId);
                attestationListModel.Competences = element.GotCompetences.Select(x => _context.Competences.Find(x)).ToList();
                attestationListModels.Add(attestationListModel);
            }
            return View(attestationListModels);
        }

        public IActionResult Output()
        {
            var workers = GetLoadedWorkers();
            return View(workers);
        }

        [HttpPost]
        public IActionResult Output(List<long> ids)
        {
            var workers = GetLoadedWorkers();
            PdfHelper.GetPdfofWorkers(ids, GetLoadedWorkers());
            var stream = new FileStream(Path.Combine(_env.ContentRootPath, "Files", "workers.pdf"), FileMode.OpenOrCreate);
            return File(stream, "application/pdf", "workers.pdf");
        }

        public IActionResult DownloadAttestation(long id)
        {
            PdfHelper.GetPdfOfAttestation(id, _context);
            var stream = new FileStream(Path.Combine(_env.ContentRootPath, "Files", "attestation.pdf"), FileMode.OpenOrCreate);
            return File(stream, "application/pdf", "attestation.pdf");
        }

        private Tuple<string, List<AnswerModel>> GetProblemsAndAnswers(AttestationModel model)
        {
            var problems = new StringBuilder("");
            var answers = new List<AnswerModel>();

            for (int i = 0; i < model.Questions.Count; i++)
            {
                var answerModel = new AnswerModel();
                if (model.Commentaries[i] == null || model.Commentaries[i] == "")
                    model.Commentaries[i] = "Комментарий не добавлен";
                answerModel.Commentary = model.Commentaries[i];
                answerModel.Question = model.Questions[i];
                answerModel.IsRight = model.RightAnswers.Contains(i);
                answerModel.IsSkipped = model.SkipedAnswers.Contains(i);
                answerModel.NumberOfAsk = i + 1;
                answers.Add(answerModel);

                if (!answerModel.IsRight && !answerModel.IsSkipped)
                {
                    problems.Append($"вопрос №{i + 1}: {answerModel.Question} \n");
                }
            }

            if (problems.ToString() == "")
                problems.Append("Всё верно!");
            Tuple<string, List<AnswerModel>> tuple = new Tuple<string, List<AnswerModel>>(problems.ToString(), answers);
            return tuple;
        }

        private List<SpecificWorkerCompetencesModel> GetNewCompetences(AttestationModel model)
        {
            var specificWorkerModel = _context.SpecificWorkerCompetences.Where(x => x.WorkerId == model.WorkerId).ToList();
            var newSpecificWorkerCompetences = new List<SpecificWorkerCompetencesModel>();

            foreach (var competence in model.GotCompetences)
            {
                newSpecificWorkerCompetences.Add(new SpecificWorkerCompetencesModel { CompetenceId = competence, WorkerId = model.WorkerId });
            }

            newSpecificWorkerCompetences = newSpecificWorkerCompetences.Union(specificWorkerModel).Distinct(new SpecificWorkerCompetencesComparer()).ToList();

            var newCompetences = newSpecificWorkerCompetences.Except(specificWorkerModel, new SpecificWorkerCompetencesComparer()).ToList();

            return newCompetences;
        }

        private void AddAnswers(List<AnswerModel> answers, AttestationModel model)
        {
            foreach (var answer in answers)
            {
                var attestationAnswerModel = new AttestationAnswerModel();

                attestationAnswerModel.Answer = answer;
                attestationAnswerModel.Attestation = model;

                model.AttestationAnswer.Add(attestationAnswerModel);
                answer.AttestationAnswer.Add(attestationAnswerModel);
                _context.Add(answer);
            }
        }
    }
}