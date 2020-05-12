using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;
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
    }
}