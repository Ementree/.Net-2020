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

namespace DotNet2020.Domain._3.Controllers
{
    public class AttestationController : Controller
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _env;
        public AttestationController(DbContext context, IWebHostEnvironment env)
        {
            _context = context;
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

        public IActionResult WorkersUpdate(int id)
        {
            var worker = _context.Set<SpecificWorkerModel>().Find(id);

            var positionId=_context.Entry(worker).Member("PositionId").CurrentValue;

            var position = _context.Set<Position>().Find(positionId);

            worker.Position = position;

            _context.Entry(worker).Collection(x => x.SpecificWorkerCompetencesModels).Load();

            var competences = _context.Set<CompetencesModel>();

            foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
            {
                specificWorkerCompetence.Competence = competences.Find(specificWorkerCompetence.CompetenceId);
            }

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

                var workerCompetences = new SpecificWorkerCompetencesModel();

                workerCompetences.Competence = competence;
                workerCompetences.Worker = workerUpdateModel.Worker;

                workerUpdateModel.Worker.SpecificWorkerCompetencesModels.Add(workerCompetences);
                specificWorkerCompetences.Add(workerCompetences);
            }

            _context.Entry(workerUpdateModel.Worker.Position).State = EntityState.Modified;
            _context.Entry(workerUpdateModel.Worker).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }

        public IActionResult WorkersAdd()
        {
            ViewBag.Competences = _context.Set<CompetencesModel>().ToList(); //для вывода всех компетенций
            return View();
        }

        [HttpPost]
        public IActionResult WorkersAdd(SpecificWorkerModel workerModel, List<long> competences)
        {
            ViewBag.Competences = _context.Set<CompetencesModel>().ToList(); //для вывода всех компетенций

            foreach (var competenceId in competences)
            {
                var competence = _context.Set<CompetencesModel>().Find(competenceId);

                var workerCompetences = new SpecificWorkerCompetencesModel();

                workerCompetences.Competence = competence;
                workerCompetences.Worker = workerModel;

                workerModel.SpecificWorkerCompetencesModels.Add(workerCompetences);
            }

            _context.Set<SpecificWorkerModel>().Add(workerModel);
            _context.SaveChanges();

            return RedirectToAction("Workers");
        }

        public IActionResult WorkersRemove(int id)
        {
            var item = _context.Set<SpecificWorkerModel>().Find(id);
            if (item != null)
                _context.Set<SpecificWorkerModel>().Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Workers");
        }
        #endregion
        #region Competences

        public IActionResult Competences()
        {
            var competences = _context.Set<CompetencesModel>();
            return View(competences);
        }

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
                case Models.CompetenceActions.AddContent:
                    competence.Content.Add(competenceUpdateModel.Content);
                    break;

                case Models.CompetenceActions.RemoveCompetence:
                    _context.Set<CompetencesModel>().Remove(competence);
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
            var grades = _context.Set<GradesModel>().ToList();

            foreach (var grade in grades)
            {
                _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
                foreach (var gradeCompetences in grade.GradesCompetences)
                {
                    gradeCompetences.Competence = _context.Set<CompetencesModel>().Find(gradeCompetences.CompetenceId);
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
            _context.Set<GradesModel>().Add(gradeModel);
            _context.SaveChanges();
            return RedirectToAction("GradesManage", new { id = gradeModel.Id });
        }

        public IActionResult GradesManage(long id)
        {
            var grade = _context.Set<GradesModel>().Find(id);
            _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
            var gradeUpdateModel = new GradeUpdateModel { GradeModel = grade, Competences = _context.Set<CompetencesModel>().ToList() };
            gradeUpdateModel.Grades = GetLoadedGrades();
            gradeUpdateModel.Grades.Remove(grade);
            gradeUpdateModel.GradeToGrades = _context.Set<GradeToGradeModel>().ToList();
            ViewBag.UpdatedGrade = grade;
            return View(gradeUpdateModel);
        }

        [HttpPost]
        public IActionResult GradesManage(long id, GradeUpdateModel gradeUpdateModel)
        {
            var grade = _context.Set<GradesModel>().Find(id);

            switch (gradeUpdateModel.Action)
            {
                case GradeActions.SetCompetences:
                    var oldGradesCompetences = _context.Set<GradeCompetencesModel>().Where(x => x.GradeId == id);
                    _context.Set<GradeCompetencesModel>().RemoveRange(oldGradesCompetences);

                    foreach (var competenceId in gradeUpdateModel.NewCompetencesIds)
                    {
                        var gradeCompetence = new GradeCompetencesModel();

                        gradeCompetence.Competence = _context.Set<CompetencesModel>().Find(competenceId);
                        gradeCompetence.Grade = grade;

                        grade.GradesCompetences.Add(gradeCompetence);

                        _context.Set<GradeCompetencesModel>().Add(gradeCompetence);
                    }
                    _context.SaveChanges();

                    break;
                case GradeActions.RemoveGrade:
                    _context.Set<GradesModel>().Remove(grade);
                    _context.SaveChanges();
                    return RedirectToAction("Grades");
                case GradeActions.AddGrade:
                    var oldGrades = _context.Set<GradeToGradeModel>().Where(x => x.GradeId == id);
                    _context.Set<GradeToGradeModel>().RemoveRange(oldGrades);
                    foreach (var newGradeId in gradeUpdateModel.NewGradesIds)
                    {
                        _context.Set<GradeToGradeModel>().Add(new GradeToGradeModel { GradeId = id, NextGradeId = newGradeId });
                    }
                    _context.SaveChanges();
                    break;
            }

            _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
            gradeUpdateModel.GradeModel = grade;
            gradeUpdateModel.Competences = _context.Set<CompetencesModel>().ToList();
            gradeUpdateModel.Grades = GetLoadedGrades();
            gradeUpdateModel.Grades.Remove(grade);
            gradeUpdateModel.GradeToGrades = _context.Set<GradeToGradeModel>().ToList();
            ViewBag.UpdatedGrade = grade;
            return View(gradeUpdateModel);
        }

        #endregion
        #region Questions
        public IActionResult Questions()
        {
            var competences = _context.Set<CompetencesModel>();
            return View(competences);
        }

        public IActionResult QuestionsManage(long id)
        {
            var questions = _context.Set<CompetencesModel>().Find(id).Questions;
            var questionUpdateModel = new QuestionUpdateModel { Questions = questions };
            return View(questionUpdateModel);
        }

        [HttpPost]
        public IActionResult QuestionsManage(long id, QuestionUpdateModel questionUpdateModel)
        {
            var competence = _context.Set<CompetencesModel>().Find(id);
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
            attestation.Competences = _context.Set<CompetencesModel>().ToList();

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
                    model.Workers = GetLoadedWorkers();
                    break;

                case AttestationAction.CompetencesChose: //вывести таблицу компетенций
                    model.Workers = GetLoadedWorkers();
                    model.Competences = _context.Set<CompetencesModel>().ToList();
                    model.Grades = GetLoadedGrades();

                    var worker = model.Workers.Where(x=>x.Id==model.WorkerId).FirstOrDefault();
                    foreach (var item in worker.SpecificWorkerCompetencesModels)
                    {
                        model.Competences.Remove(item.Competence);
                    }
                    
                    break;

                case AttestationAction.GradeChose: //вывести таблицу грейдов
                    model.Workers = GetLoadedWorkers();
                    model.Competences = _context.Set<CompetencesModel>().ToList();
                    model.Grades = GetLoadedGrades();

                    worker = model.Workers.Where(x => x.Id == model.WorkerId).FirstOrDefault();
                    var workerCompetences = new List<CompetencesModel>();
                    foreach (var item in worker.SpecificWorkerCompetencesModels)
                    {
                        workerCompetences.Add(item.Competence);
                    }

                    var workerGrades = GetWorkerGrades(workerCompetences, model.Grades, _context.Set<GradeToGradeModel>().ToList());

                    foreach (var element in workerGrades)
                    {
                        if (model.Grades.Contains(element))
                        {
                            model.Grades.Remove(element);
                        }
                    }
                    break;

                case AttestationAction.AttestationByCompetences: //вывести окно аттестации по компетенциям
                    worker = GetLoadedWorkers().Where(x => x.Id == model.WorkerId).FirstOrDefault();
                    workerCompetences = new List<CompetencesModel>();

                    foreach (var item in worker.SpecificWorkerCompetencesModels)
                        workerCompetences.Add(item.Competence);

                    var questions = new List<string>();
                    var testedCompetences = new List<CompetencesModel>();
                    foreach (var competenceId in model.IdsTestedCompetences)
                    {
                        var competence = _context.Set<CompetencesModel>().Find(competenceId);
                        if (!workerCompetences.Contains(competence))
                        {
                            testedCompetences.Add(competence);
                            questions = questions.Union(competence.Questions).ToList();
                        }
                    }
                    questions = questions.Distinct().ToList();
                    model.Questions = questions;
                    model.TestedCompetences = testedCompetences;
                    break;

                case AttestationAction.AttestationByGrade: //вывести окно аттестации по грейдам
                    worker = GetLoadedWorkers().Where(x => x.Id == model.WorkerId).FirstOrDefault();
                    var questionsForGrade = new List<string>();
                    var gradeId = model.GradeId.Value;

                    var testedGradeCompetences = _context.Set<GradeCompetencesModel>().Where(x => x.GradeId == gradeId).ToList();
                    testedCompetences = new List<CompetencesModel>();

                    workerCompetences = new List<CompetencesModel>();

                    foreach (var item in worker.SpecificWorkerCompetencesModels)
                        workerCompetences.Add(item.Competence);

                    foreach (var testedGradeCompetence in testedGradeCompetences)
                    {
                        var competence = _context.Set<CompetencesModel>().Find(testedGradeCompetence.CompetenceId);
                        if (!workerCompetences.Contains(competence))
                        {
                            questionsForGrade = questionsForGrade.Union(competence.Questions).ToList();
                            testedCompetences.Add(competence);
                        }
                    }

                    questionsForGrade = questionsForGrade.Distinct().ToList();
                    model.TestedCompetences = testedCompetences;
                    model.Questions = questionsForGrade;

                    break;

                case AttestationAction.Finished: //сохранить результаты
                    model.Grades = GetLoadedGrades();
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
                        _context.Set<SpecificWorkerCompetencesModel>().Add(newCompetence);
                        gotCompetences.Add(newCompetence.CompetenceId);
                    }

                    workerCompetences = new List<CompetencesModel>();
                    worker = _context.Set<SpecificWorkerModel>().Where(x => x.Id == model.WorkerId).FirstOrDefault();
                    foreach (var item in worker.SpecificWorkerCompetencesModels.Union(newCompetences))
                    {
                        workerCompetences.Add(item.Competence);
                    }

                    workerGrades = GetWorkerGrades(workerCompetences, model.Grades, _context.Set<GradeToGradeModel>().ToList());

                    foreach (var element in workerGrades)
                    {
                        if (model.Grades.Contains(element))
                        {
                            model.Grades.Remove(element);
                        }
                    }

                    model.NextMoves = GetNextMoves(model.Grades);
                    model.GotCompetences = gotCompetences;

                    AddAnswers(answers, model);

                    _context.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction("AttestationList");
            }
            return View(model);
        }
        #endregion

        private string GetNextMoves(List<GradesModel> nextGrades)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in nextGrades)
            {
                builder.Append(item.Grade + "\n");
            }
            return builder.ToString();
        }
        private List<SpecificWorkerModel> GetLoadedWorkers()
        {
            var workers = _context.Set<SpecificWorkerModel>().ToList();
            foreach (var worker in workers)
            {
                _context.Entry(worker).Collection(x => x.SpecificWorkerCompetencesModels).Load();
                foreach (var specificWorkerCompetence in worker.SpecificWorkerCompetencesModels)
                {
                    specificWorkerCompetence.Competence = _context.Set<CompetencesModel>().Find(specificWorkerCompetence.CompetenceId);
                }
                var positionId=_context.Entry(worker).Member("PositionId").CurrentValue;
                var position=_context.Set<Position>().Find((int)positionId);
                worker.Position = position;
            }
            return workers;
        }

        private List<GradesModel> GetLoadedGrades()
        {
            var grades = _context.Set<GradesModel>().ToList();
            foreach (var grade in grades)
            {
                _context.Entry(grade).Collection(x => x.GradesCompetences).Load();
                foreach (var gradeCompetences in grade.GradesCompetences)
                {
                    gradeCompetences.Competence = _context.Set<CompetencesModel>().Find(gradeCompetences.CompetenceId);
                }
            }
            return grades;
        }

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

        public IActionResult Output()
        {
            var workers = GetLoadedWorkers();
            return View(workers);
        }

        [HttpPost]
        public IActionResult Output(List<long> ids)
        {
            var stream = PdfHelper.GetPdfofWorkers(ids, GetLoadedWorkers());
            return File(stream, "application/pdf", "workers.pdf");
        }

        public IActionResult DownloadAttestation(long id)
        {
            PdfHelper.GetPdfOfAttestation(id, _context);
            var stream = PdfHelper.GetPdfOfAttestation(id, _context);
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
            var specificWorkerModel = _context.Set<SpecificWorkerCompetencesModel>().Where(x => x.WorkerId == model.WorkerId).ToList();
            var newSpecificWorkerCompetences = new List<SpecificWorkerCompetencesModel>();

            foreach (var competence in model.GotCompetences)
            {
                newSpecificWorkerCompetences.Add(new SpecificWorkerCompetencesModel { CompetenceId = competence, WorkerId = (int)model.WorkerId });
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

        public List<GradesModel> GetLowerGrades(long gradeId, List<GradesModel> grades, List<GradeToGradeModel> links)
        {
            var neededLinks = links.Where(l => l.NextGradeId == gradeId);
            var result = new List<GradesModel>();
            foreach (var link in neededLinks)
            {
                var currentGrade = grades.FirstOrDefault(g => g.Id == link.GradeId);
                if (!result.Contains(currentGrade))
                {
                    result.Add(currentGrade);
                    result.AddRange(GetLowerGrades(currentGrade.Id, grades, links));
                }
            }

            return result;
        }

        public List<CompetencesModel> GetLowerCompetences(long gradeId, List<GradesModel> grades, List<GradeToGradeModel> links)
        {
            var neededGrades = GetLowerGrades(gradeId, grades, links);
            neededGrades.Add(grades.FirstOrDefault(x => x.Id == gradeId));
            var result = new List<CompetencesModel>();
            foreach (var grade in neededGrades)
            {
                var gradeCompetences = grade.GradesCompetences;
                foreach (var competence in gradeCompetences.Where(competence => !result.Contains(competence.Competence)))
                    result.Add(competence.Competence);
            }

            return result;
        }

        public List<GradesModel> GetWorkerGrades(List<CompetencesModel> workerCompetences, List<GradesModel> grades,
            List<GradeToGradeModel> links)
        {
            var result = new List<GradesModel>();
            foreach (var grade in grades)
            {
                var gradeCompetences = GetLowerCompetences(grade.Id, grades, links);
                if (!gradeCompetences.Except(workerCompetences).Any()) //чекает что у рабочего есть все компетенции грейда
                    result.Add(grade);
            }
            return result;
        }
    }
}