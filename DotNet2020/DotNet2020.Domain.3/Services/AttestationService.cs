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
            attestation.Workers = _workerService.GetLoadedWorkers(); //работники
            attestation.Competences = _context.Set<CompetencesModel>().ToList(); //компетенции

            var worker = attestation.Workers.Where(x => x.Id == attestation.WorkerId).FirstOrDefault(); //получаем работника

            foreach (var workerCompetence in worker.SpecificWorkerCompetencesModels) //вывести все компетенции, кроме тех, что есть у работника
            {
                attestation.Competences.Remove(workerCompetence.Competence);
            }
            return attestation;
        }

        public AttestationModel CreateGradeTable(AttestationModel attestation)
        {
            attestation.Workers = _workerService.GetLoadedWorkers();
            attestation.Grades = _gradeService.GetLoadedGrades();

            var worker = _workerService.GetWorker((int)attestation.WorkerId); //получаем работника

            var workerCompetences = new List<CompetencesModel>(); //лист из компетенций работника

            foreach (var item in worker.SpecificWorkerCompetencesModels)
            {
                workerCompetences.Add(item.Competence);
            } //заполняем его

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, attestation.Grades, _context.Set<GradeToGradeModel>().ToList()); //список имеющихся грейдов

            if (workerGrades.Count() != 0) //если список не пустой
            {
                var max = workerGrades.Max(x => x.GradesCompetences.Count()); //определяем максимальное количество компетенций у имеющихся грейдов

                var currentGrade = workerGrades.Where(x => x.GradesCompetences.Count() == max).FirstOrDefault(); //получаем грейд с максимальным количеством компетенций

                foreach (var element in workerGrades) //на выход имеем список грейдов, которых нет у сотрудника
                {
                    if (attestation.Grades.Contains(element))
                    {
                        attestation.Grades.Remove(element);
                    }
                }
                attestation.Grades.Add(currentGrade); //добавить текущий грейд для переаттестации
            }
            return attestation;
        }

        public AttestationModel CreateAttestationByCompetences(AttestationModel attestation)
        {
            bool isValid = true; //возможно ли вообще провести аттестацию (имеется как минимум 1 вопрос каждой сложности)
            var worker = _workerService.GetWorker((int)attestation.WorkerId); //получаем работника
            var workerCompetences = new List<CompetencesModel>(); //получаем компетенции работника

            foreach (var item in worker.SpecificWorkerCompetencesModels)
                workerCompetences.Add(item.Competence); //заполняем этот лист

            var questions = new List<string>(); //лист для вопросов к аттестации

            var testedCompetences = new List<CompetencesModel>(); //лист тестируемых компетенций

            foreach (var competenceId in attestation.IdsTestedCompetences) //заполняем лист тестируемых аттестаций
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
                .ToList(); //получаем вопросы к тестируемым компетенциям, и isValid

            if (isValid) //если возможно провести аттестацию
            {
                questions = questions.Distinct().ToList(); //убираем повторяющиеся
                if (_context.Set<AttestationModel>().Any(x => x.WorkerId == attestation.WorkerId)) //если у работника до этого были аттестации
                {
                    questions.AddRange(_questionService.GetFiftyPercentOfWrongQuestionsFromlastAttestation(attestation));
                }

                attestation.Questions = questions; //вопросы, которые будут выведены
                attestation.TestedCompetences = testedCompetences; //лист компетенций, по которым проходит аттестация
            }
            else //случай, когда нельзя провести аттестацию
            {
                attestation.Action = AttestationAction.NotEnoughQuestion;
                return attestation;
            }
            return attestation;
        }

        public AttestationModel CreateAttestationByGrades(AttestationModel attestation, out bool IsReattestation)
        {
            bool isValid = true;
            var grades = _gradeService.GetLoadedGrades(); //получаем все грейды
            IsReattestation = false; //переменная, проверяющая, проходит ли переаттестация

            var worker = _workerService.GetWorker((int)attestation.WorkerId); //получаем работника

            var gradeToGrades = _context.Set<GradeToGradeModel>().ToList(); //получаем список связей грейдов
            var questionsForGrade = new List<string>(); //создаем список для вопросов
            var gradeId = attestation.GradeId.Value; //получаем id тестируемого грейда
            var testedGradeCompetences = _context.Set<GradeCompetencesModel>()
                .Where(x => x.GradeId == gradeId)
                .ToList(); //лист связей грейдов и компетенций, состоящий только из связей текущего грейда

            var testedCompetences = new List<CompetencesModel>(); //тестируемые компетенции

            var workerCompetences = new List<CompetencesModel>(); //текущие компетенции

            foreach (var item in worker.SpecificWorkerCompetencesModels)
                workerCompetences.Add(item.Competence); //получаем текущие компетенции

            foreach (var testedGradeCompetence in testedGradeCompetences) //добавляем в тестируемые компетенции все, кроме тех, что есть у работника
            {
                var competence = _context.Set<CompetencesModel>().Find(testedGradeCompetence.CompetenceId);
                if (!workerCompetences.Contains(competence))
                {
                    testedCompetences.Add(competence);
                }
            }

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, grades, _context.Set<GradeToGradeModel>().ToList()); //список грейдов, которые есть у работника

            if (workerGrades.Count() != 0) //если не пустой
            {
                var max = workerGrades.Max(x => x.GradesCompetences.Count());
                var currentGrade = workerGrades.Where(x => x.GradesCompetences.Count() == max).FirstOrDefault(); //кладём "максимальный" грейд

                if (currentGrade.Id == attestation.GradeId.Value) //если тестируемый грейд совпадает с максимальным грейдом, тогда происходит реаттетсация
                    IsReattestation = true;
            }

            if (IsReattestation) //если происходит реаттестация
            {
                if (!gradeToGrades.Where(x => x.NextGradeId == gradeId).Any()) //если выбранный грейд "первый"(перед этим грейдом нет других)
                {
                    var grade = grades.Where(x => x.Id == gradeId).FirstOrDefault(); //получаем этот грейд
                    testedCompetences.AddRange(grade.GradesCompetences.Select(x => x.Competence)); //и кладём в тестируемые компетенции компетенции этого грейда
                }
                else //если выбранный грейд не первый
                {
                    var previousGradesIds = gradeToGrades //получаем id предыдущих грейдов от текущего грейда
                        .Where(x => x.NextGradeId == gradeId)
                        .Select(x => x.GradeId)
                        .ToList();

                    var previousGrades = grades //получаем предыдущие грейды от текущего грейда
                        .Where(x => previousGradesIds.Contains(x.Id))
                        .ToList();

                    var previousCompetences = previousGrades //получаем не повторяющиеся предыдущие компетенции предыдущих грейдов 
                        .SelectMany(x => x.GradesCompetences)
                        .Select(x => x.Competence)
                        .Distinct()
                        .ToList();

                    List<CompetencesModel> needToReattestateCompetences = new List<CompetencesModel>(); //лист компетенций для реаттестаций

                    if (previousGrades.Count == 1) //если только 1 предыдущий грейд
                    {
                        needToReattestateCompetences = workerCompetences //тогда кладём в лист все предыдущие компетенции
                            .Where(x => !previousCompetences
                            .Any(y => x.Id == y.Id))
                            .ToList();
                    }
                    else //если больше 1 предыдущего
                    {
                        Dictionary<CompetencesModel, int> dict = new Dictionary<CompetencesModel, int>(); //словарь компетенции и количества вхождений

                        foreach (var previousCompetence in previousCompetences) //каждой компетенции присваиваем 0 вхождений
                        {
                            dict[previousCompetence] = 0;
                        }

                        foreach (var previousGrade in previousGrades) //считаем количество компетенций предыдущих грейдов
                        {
                            var competences = previousGrade.GradesCompetences.Select(x => x.Competence).ToList();
                            foreach (var competence in competences)
                            {
                                dict[competence]++;
                            }
                        }

                        foreach (var element in dict) //добавляем все компетенцие, которые вошли менее КОЛИЧЕСТВА грейдов раз
                        {
                            if (element.Value != previousGrades.Count())
                            {
                                needToReattestateCompetences.Add(element.Key);
                            }
                        }
                    }
                    testedCompetences.AddRange(needToReattestateCompetences); //добавляем их в список компетенций для переаттестации
                }
            }

            questionsForGrade = _questionService //получаем вопросы для грейда исходя из тестируемых компетенций и проверяем возможность проведения
                .GetCompetencesAttestationQuestions(testedCompetences, out isValid)
                .Select(x => x.Question)
                .ToList();

            if (!isValid) //если нельзя провести, не проводим
            {
                attestation.Action = AttestationAction.NotEnoughQuestion;
                return attestation;
            }

            if (_context.Set<AttestationModel>().Any(x => x.WorkerId == attestation.WorkerId)) //если есть предыдущие аттестации, добавляем 50% от неправильных
            {
                questionsForGrade.AddRange(_questionService.GetFiftyPercentOfWrongQuestionsFromlastAttestation(attestation));
            }

            questionsForGrade = questionsForGrade.Distinct().ToList(); //удаляем повторения

            attestation.TestedCompetences = testedCompetences; //выводим
            attestation.Questions = questionsForGrade;
            return attestation;
        }

        public void FinishAttestation(AttestationModel attestation)
        {
            attestation.Grades = _gradeService.GetLoadedGrades(); //получаем все грейды

            var tuple = GetProblemsAndAnswers(attestation); //получаем выявленные проблемы и ответы
            attestation.Problems = tuple.Item1;
            var answers = tuple.Item2;
            attestation.Date = DateTime.Today;

            if (attestation.GradeId != null) //если мы проводили аттестацию про грейду
            {
                if (attestation.IsGotGrade != null)
                    attestation.GotCompetences = attestation.IdsTestedCompetences; //если грейд получен, добавляем все полученные id 
                else
                    attestation.GotCompetences = new List<long>(); //нежели обнуляем его
            }

            var gotCompetencesIds = new List<long>(); //список id полученных компетенций

            var newCompetences = GetNewCompetences(attestation); //возвращает заполненный новыми компетенциями лист связей работника и компетений

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
                    _context.Set<SpecificWorkerCompetencesModel>().Add(newCompetence); //привязка компетенций сотруднику
                    gotCompetencesIds.Add(newCompetence.CompetenceId); //добавление в лист полученных id
                }
            }



            var workerCompetences = new List<CompetencesModel>(); //компетенции работника

            var worker = _workerService.GetWorker((int)attestation.WorkerId); //сам работник

            foreach (var item in worker.SpecificWorkerCompetencesModels.Union(newCompetences)) //добавляем в компетенции работника старые и новые компетенции
            {
                workerCompetences.Add(item.Competence);
            }

            var workerGrades = _gradeService.GetWorkerGrades(workerCompetences, attestation.Grades, _context.Set<GradeToGradeModel>().ToList()); //получить все грейды работника

            foreach (var element in workerGrades) //удаляем из списка грейдов имеющиеся грейды
            {
                if (attestation.Grades.Contains(element))
                {
                    attestation.Grades.Remove(element);
                }
            }

            attestation.NextMoves = _gradeService.GetNextGrades(attestation.Grades); //получаем следующие грейды, которые может получить работник

            attestation.GotCompetences = gotCompetencesIds; //список полученных компетенций

            _answerService.AddAnswers(answers, attestation); //добавляем ответы

            _context.Add(attestation);
            _context.SaveChanges();
        }
    }
}
