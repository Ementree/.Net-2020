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

        public List<QuestionModel> GetCompetencesAttestationQuestions(List<CompetencesModel> competences)
        {
            var result = new List<QuestionModel>();
            foreach (var competence in competences)
                result.AddRange(GetOneCompetenceAttestationQuestions(competence));
            return result;
        }

        public List<QuestionModel> GetOneCompetenceAttestationQuestions(CompetencesModel compentence)
        {

            var questionsIds = _context.Set<CompetenceQuestionsModel>().Where(x => x.CompetenceId == compentence.Id).Select(x => x.QuestionId).ToList();
            var questions = _context.Set<QuestionModel>().Where(x => questionsIds.Contains(x.Id)).ToList();

            var result = new List<QuestionModel>
            {
                GetRandomQuestion(questions, Easy),
                GetRandomQuestion(questions, Medium),
                GetRandomQuestion(questions, Hard)
            };
            return result;
        }

        public QuestionModel GetRandomQuestion(List<QuestionModel> questions, string difficulty)
        {
            var rand = new Random();
            var complexityId = _context.Set<QuestionComplexityModel>().Where(x => x.Value == difficulty).Select(x => x.Id).FirstOrDefault();
            var neededQuestions = questions
                .Where(q => q.ComplexityId == complexityId)
                .ToList();
            var number = rand.Next(0, neededQuestions.Count());
            return neededQuestions[number];
        }
    }
}