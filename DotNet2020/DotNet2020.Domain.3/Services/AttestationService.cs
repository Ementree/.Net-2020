using DotNet2020.Domain._3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._3.Services
{
    public class AttestationService
    {
        private readonly DbContext _context;
        private readonly WorkerService _workerService;
        private readonly GradeService _gradeService;
        private readonly QuestionService _questionService;
        private readonly AnswerService _answerService;
        public AttestationService(DbContext context)
        {
            _context = context;
            _workerService = new WorkerService(context);
            _gradeService = new GradeService(context);
            _questionService = new QuestionService(context);
            _answerService = new AnswerService(context);
        }

        public Tuple<string, List<AnswerModel>> GetProblemsAndAnswers(AttestationModel model)
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

        public List<SpecificWorkerCompetencesModel> GetNewCompetences(AttestationModel model)
        {
            var specificWorkerModel = _context.Set<SpecificWorkerCompetencesModel>().Where(x => x.WorkerId == model.WorkerId).ToList();
            var newSpecificWorkerCompetences = new List<SpecificWorkerCompetencesModel>();

            if (model.GotCompetences == null)
            {
                model.GotCompetences = new List<long>();
            }

            foreach (var competence in model.GotCompetences)
            {
                newSpecificWorkerCompetences.Add(new SpecificWorkerCompetencesModel { CompetenceId = competence, WorkerId = (int)model.WorkerId });
            }

            newSpecificWorkerCompetences = newSpecificWorkerCompetences.Union(specificWorkerModel).Distinct(new SpecificWorkerCompetencesComparer()).ToList();

            var newCompetences = newSpecificWorkerCompetences.Except(specificWorkerModel, new SpecificWorkerCompetencesComparer()).ToList();

            return newCompetences;
        }

        public AttestationModel CreateCompetenceTable(AttestationModel attestation)
        {
            attestation.Workers = _workerService.GetLoadedWorkers();
            attestation.Competences = _context.Set<CompetencesModel>().ToList();

            var worker = attestation.Workers.Where(x => x.Id == attestation.WorkerId).FirstOrDefault();

            foreach (var workerCompetence in worker.SpecificWorkerCompetencesModels)
            {
                attestation.Competences.Remove(workerCompetence.Competence);
            }
            return attestation;
        }

        public AttestationModel CreateGradeTable(AttestationModel attestation)
        {
            attestation.Workers = _workerService.GetLoadedWorkers();
            attestation.Grades = _gradeService.GetLoadedGrades();

            var worker = _workerService.GetWorker((int)attestation.WorkerId);

            var workerCompetences = new List<CompetencesModel>();

            foreach (var item in worker.SpecificWorkerCompetencesModels)
            {
                workerCompetences.Add(item.Competence);
            } //заполняем его

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, attestation.Grades, _context.Set<GradeToGradeModel>().ToList());

            if (workerGrades.Count() != 0)
            {
                var max = workerGrades.Max(x => x.GradesCompetences.Count());

                var currentGrade = workerGrades.Where(x => x.GradesCompetences.Count() == max).FirstOrDefault();

                foreach (var element in workerGrades)
                {
                    if (attestation.Grades.Contains(element))
                    {
                        attestation.Grades.Remove(element);
                    }
                }
                attestation.Grades.Add(currentGrade);
            }
            return attestation;
        }

        public AttestationModel CreateAttestationByCompetences(AttestationModel attestation)
        {
            bool isValid = true;
            var worker = _workerService.GetWorker((int)attestation.WorkerId);
            var workerCompetences = new List<CompetencesModel>();

            foreach (var item in worker.SpecificWorkerCompetencesModels)
                workerCompetences.Add(item.Competence);

            var questions = new List<string>();

            var testedCompetences = new List<CompetencesModel>();

            foreach (var competenceId in attestation.IdsTestedCompetences)
            {
                var competence = _context.Set<CompetencesModel>().Find(competenceId);
                if (!workerCompetences.Contains(competence))
                {
                    testedCompetences.Add(competence);
                }
            }

            questions = _questionService
                .GetCompetencesAttestationQuestions(testedCompetences, out isValid)
                .Select(x => x.Question)
                .ToList();

            if (isValid)
            {
                questions = questions.Distinct().ToList();
                if (_context.Set<AttestationModel>().Any(x => x.WorkerId == attestation.WorkerId))
                {
                    questions.AddRange(_questionService.GetFiftyPercentOfWrongQuestionsFromlastAttestation(attestation));
                }

                attestation.Questions = questions;
                attestation.TestedCompetences = testedCompetences;
            }
            else
            {
                attestation.Action = AttestationAction.NotEnoughQuestion;
                return attestation;
            }
            return attestation;
        }

        public AttestationModel CreateAttestationByGrades(AttestationModel attestation, out bool IsReattestation)
        {
            bool isValid = true;
            var grades = _gradeService.GetLoadedGrades();
            IsReattestation = false;

            var worker = _workerService.GetWorker((int)attestation.WorkerId);

            var gradeToGrades = _context.Set<GradeToGradeModel>().ToList();
            var questionsForGrade = new List<string>();
            var gradeId = attestation.GradeId.Value;
            var testedGradeCompetences = _context.Set<GradeCompetencesModel>()
                .Where(x => x.GradeId == gradeId)
                .ToList();

            var testedCompetences = new List<CompetencesModel>();

            var workerCompetences = new List<CompetencesModel>();

            foreach (var item in worker.SpecificWorkerCompetencesModels)
                workerCompetences.Add(item.Competence);

            foreach (var testedGradeCompetence in testedGradeCompetences)
            {
                var competence = _context.Set<CompetencesModel>().Find(testedGradeCompetence.CompetenceId);
                if (!workerCompetences.Contains(competence))
                {
                    testedCompetences.Add(competence);
                }
            }

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, grades, _context.Set<GradeToGradeModel>().ToList());

            if (workerGrades.Count() != 0)
            {
                var max = workerGrades.Max(x => x.GradesCompetences.Count());
                var currentGrade = workerGrades.Where(x => x.GradesCompetences.Count() == max).FirstOrDefault();

                if (currentGrade.Id == attestation.GradeId.Value)
                    IsReattestation = true;
            }

            if (IsReattestation)
            {
                if (!gradeToGrades.Where(x => x.NextGradeId == gradeId).Any())
                {
                    var grade = grades.Where(x => x.Id == gradeId).FirstOrDefault();
                    testedCompetences.AddRange(grade.GradesCompetences.Select(x => x.Competence));
                }
                else
                {
                    var previousGradesIds = gradeToGrades
                        .Where(x => x.NextGradeId == gradeId)
                        .Select(x => x.GradeId)
                        .ToList();

                    var previousGrades = grades
                        .Where(x => previousGradesIds.Contains(x.Id))
                        .ToList();

                    var previousCompetences = previousGrades
                        .SelectMany(x => x.GradesCompetences)
                        .Select(x => x.Competence)
                        .Distinct()
                        .ToList();

                    List<CompetencesModel> needToReattestateCompetences = new List<CompetencesModel>();

                    if (previousGrades.Count == 1)
                    {
                        needToReattestateCompetences = workerCompetences
                            .Where(x => !previousCompetences
                            .Any(y => x.Id == y.Id))
                            .ToList();
                    }
                    else
                    {
                        Dictionary<CompetencesModel, int> dict = new Dictionary<CompetencesModel, int>();

                        foreach (var previousCompetence in previousCompetences)
                        {
                            dict[previousCompetence] = 0;
                        }

                        foreach (var previousGrade in previousGrades)
                        {
                            var competences = previousGrade.GradesCompetences.Select(x => x.Competence).ToList();
                            foreach (var competence in competences)
                            {
                                dict[competence]++;
                            }
                        }

                        foreach (var element in dict)
                        {
                            if (element.Value != previousGrades.Count())
                            {
                                needToReattestateCompetences.Add(element.Key);
                            }
                        }
                    }
                    testedCompetences.AddRange(needToReattestateCompetences);
                }
            }

            questionsForGrade = _questionService
                .GetCompetencesAttestationQuestions(testedCompetences, out isValid)
                .Select(x => x.Question)
                .ToList();

            if (!isValid)
            {
                attestation.Action = AttestationAction.NotEnoughQuestion;
                return attestation;
            }

            if (_context.Set<AttestationModel>().Any(x => x.WorkerId == attestation.WorkerId))
            {
                questionsForGrade.AddRange(_questionService.GetFiftyPercentOfWrongQuestionsFromlastAttestation(attestation));
            }

            questionsForGrade = questionsForGrade.Distinct().ToList();

            attestation.TestedCompetences = testedCompetences;
            attestation.Questions = questionsForGrade;
            return attestation;
        }

        public void FinishAttestation(AttestationModel attestation)
        {
            attestation.Grades = _gradeService.GetLoadedGrades();

            var tuple = GetProblemsAndAnswers(attestation);
            attestation.Problems = tuple.Item1;
            var answers = tuple.Item2;
            attestation.Date = DateTime.Today;

            if (attestation.GradeId != null)
            {
                if (attestation.IsGotGrade != null)
                    attestation.GotCompetences = attestation.IdsTestedCompetences;
                else
                    attestation.GotCompetences = new List<long>();
            }

            var gotCompetencesIds = new List<long>();

            var newCompetences = GetNewCompetences(attestation);

            if (attestation.ReAttestation)
            {
                var lostedCompetencesIds = attestation.IdsTestedCompetences.Except(attestation.GotCompetences);
                var lostedCompetences = _context.Set<SpecificWorkerCompetencesModel>()
                    .Where(x => lostedCompetencesIds.Contains(x.CompetenceId) && x.WorkerId == attestation.WorkerId);
                _context.Set<SpecificWorkerCompetencesModel>().RemoveRange(lostedCompetences);
            }
            else
            {
                foreach (var newCompetence in newCompetences)
                {
                    _context.Set<SpecificWorkerCompetencesModel>().Add(newCompetence);
                    gotCompetencesIds.Add(newCompetence.CompetenceId);
                }
            }



            var workerCompetences = new List<CompetencesModel>();

            var worker = _workerService.GetWorker((int)attestation.WorkerId);

            foreach (var item in worker.SpecificWorkerCompetencesModels.Union(newCompetences))
            {
                workerCompetences.Add(item.Competence);
            }

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, attestation.Grades, _context.Set<GradeToGradeModel>().ToList());

            foreach (var element in workerGrades)
            {
                if (attestation.Grades.Contains(element))
                {
                    attestation.Grades.Remove(element);
                }
            }

            attestation.NextMoves = _gradeService.GetNextGrades(attestation.Grades);

            attestation.GotCompetences = gotCompetencesIds;

            _answerService.AddAnswers(answers, attestation);

            _context.Add(attestation);
            _context.SaveChanges();
        }
    }
}
