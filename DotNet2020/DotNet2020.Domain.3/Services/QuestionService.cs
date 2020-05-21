using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Services
{
    public class QuestionService
    {
        public const string Easy = "легкий";
        public const string Medium = "средний";
        public const string Hard = "тяжелый";
        private readonly DbContext _context;

        public QuestionService(DbContext context)
        {
            _context = context;
        }

        public List<QuestionModel> GetCompetencesAttestationQuestions(List<CompetencesModel> competences, out bool isValid)
        {
            isValid = true;
            var result = new List<QuestionModel>();
            foreach (var competence in competences)
                result.AddRange(GetOneCompetenceAttestationQuestions(competence, out isValid));
            return result;
        }

        public List<QuestionModel> GetOneCompetenceAttestationQuestions(CompetencesModel compentence, out bool isValid)
        {
            isValid = true;
            var questionsIds = _context.Set<CompetenceQuestionsModel>().Where(x => x.CompetenceId == compentence.Id).Select(x => x.QuestionId).ToList();
            var questions = _context.Set<QuestionModel>().Where(x => questionsIds.Contains(x.Id)).ToList();

            var result = new List<QuestionModel>();
            result.AddRange(GetRandomQuestion(questions, Easy));
            result.AddRange(GetRandomQuestion(questions, Medium));
            result.AddRange(GetRandomQuestion(questions, Hard));

            if (result.Count < 3)
                isValid = false;
            return result;
        }

        public List<QuestionModel> GetRandomQuestion(List<QuestionModel> questions, string difficulty)
        {
            var result = new List<QuestionModel>();
            var rnd = new Random();
            var complexityId = _context.Set<QuestionComplexityModel>().Where(x => x.Value == difficulty).Select(x => x.Id).FirstOrDefault();
            var neededQuestions = questions
                .Where(q => q.ComplexityId == complexityId)
                .ToList();
            if (neededQuestions.Count == 0)
            {
                return result;
            }
            var number = rnd.Next(0, neededQuestions.Count());
            result.Add(neededQuestions[number]);
            return result;
        }

        public List<QuestionModel> GetPreviousAttestationQuestions(AttestationModel attestation)
        {
            var questions = _context.Set<QuestionModel>().ToList();
            var attestationToAnswer = _context.Set<AttestationAnswerModel>().ToList();
            var answers = _context.Set<AnswerModel>().ToList();

            var answerIds = attestationToAnswer
                .Where(x => x.AttestationId == attestation.Id)
                .Select(x => x.AnswerId)
                .ToList();

            var neededQuestions = new List<QuestionModel>();
            foreach (var id in answerIds)
            {
                var answer = answers.Where(x => !x.IsRight).FirstOrDefault(a => a.AnswerId == id);

                if (answer == default) continue;
                var question = questions.FirstOrDefault(q => q.Question == answer.Question);
                if (question != default)
                    neededQuestions.Add(question);
            }

            var rnd = new Random();
            var result = neededQuestions
                .OrderBy(x => rnd.Next())
                .Take(neededQuestions.Count / 2)
                .ToList();
            return result;
        }

        public List<CompetenceQuestionsViewModel> GetQuestions()
        {
            var competences = _context.Set<CompetencesModel>();
            var competenceQuestionsViewModels = new List<CompetenceQuestionsViewModel>();
            var competenceQuestions = _context.Set<CompetenceQuestionsModel>().ToList();
            var questions = _context.Set<QuestionModel>().ToList();
            foreach (var competence in competences)
            {
                var viewModel = new CompetenceQuestionsViewModel(competence, competenceQuestions, questions);
                competenceQuestionsViewModels.Add(viewModel);
            }
            return competenceQuestionsViewModels;
        }

        public QuestionUpdateModel GetQuestionUpdateModel(long id)
        {
            var questionIds = _context.Set<CompetenceQuestionsModel>()
                .Where(x => x.CompetenceId == id)
                .Select(x => x.QuestionId)
                .ToList();

            var questions = _context.Set<QuestionModel>()
                .Where(x => questionIds.Contains(x.Id))
                .Select(x => x.Question)
                .ToList();

            var questionUpdateModel = new QuestionUpdateModel
            {
                Complexities = _context.Set<QuestionComplexityModel>()
                .Select(x => x.Value)
                .ToList(),
                Questions = questions
            };
            return questionUpdateModel;
        }

        public void RemoveQuestions(long id, QuestionUpdateModel questionUpdateModel)
        {
            foreach (var question in questionUpdateModel.QuestionsToRemove)
            {
                var neededToDeleteQuestionModels = _context.Set<QuestionModel>().Where(x => x.Question == question);
                var neededToDeleteIds = neededToDeleteQuestionModels.Select(x => x.Id).ToList();
                var relatedDataToDelete = _context.Set<CompetenceQuestionsModel>().
                    Where(x => x.CompetenceId == id && neededToDeleteIds.Contains(x.QuestionId)).ToList();

                _context.Set<QuestionModel>().RemoveRange(neededToDeleteQuestionModels);
                _context.Set<CompetenceQuestionsModel>().RemoveRange(relatedDataToDelete);
            }
            _context.SaveChanges();
        }

        public void AddNewQuestion(long id, QuestionUpdateModel questionUpdateModel)
        {
            var newQuestion = new QuestionModel
            {
                Question = questionUpdateModel.NewQuestion,
                ComplexityId = _context
                        .Set<QuestionComplexityModel>()
                        .Where(x => x.Value == questionUpdateModel.Complexity)
                        .Select(x => x.Id)
                        .FirstOrDefault()
            };
            _context.Set<QuestionModel>().Add(newQuestion);
            _context.SaveChanges();
            _context.Set<CompetenceQuestionsModel>().Add(new CompetenceQuestionsModel { CompetenceId = id, QuestionId = newQuestion.Id });
            _context.SaveChanges();
        }

        public List<string> GetFiftyPercentOfWrongQuestionsFromlastAttestation(AttestationModel attestation)
        {
            var questions = new List<string>();

            var lastDate = _context.Set<AttestationModel>()
                            .Where(x => x.WorkerId == attestation.WorkerId)
                            .Max(x => x.Date);

            var lastAttestation = _context.Set<AttestationModel>()
                .Where(x => x.WorkerId == attestation.WorkerId && x.Date == lastDate)
                .FirstOrDefault();

            if (lastAttestation != null)
            {
                questions.AddRange(GetPreviousAttestationQuestions(lastAttestation).Select(x => x.Question));
            }
            return questions;
        }
    }
}