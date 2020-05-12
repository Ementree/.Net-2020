using System;
using System.Collections.Generic;
using System.Linq;
using DotNet2020.Domain._3.Models;

namespace DotNet2020.Domain._3.Services
{
    public class QuestionService
    {
        public const string Easy = "easy";
        public const string Medium = "medium";
        public const string Hard = "hard";

        public List<QuestionModel> GetCompetencesAttestationQuestions(List<CompetencesModel> competences)
        {
            var result = new List<QuestionModel>();
            foreach (var competence in competences)
                result.AddRange(GetOneCompetenceAttestationQuestions(competence));
            return result;
        }

        public List<QuestionModel> GetOneCompetenceAttestationQuestions(CompetencesModel compentece)
        {
            var questions = compentece.Questions;
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
            var neededQuestions = questions
                .Where(q => q.Complexity.Value == difficulty)
                .ToList();
            var number = rand.Next(0, neededQuestions.Count() - 1);
            return neededQuestions[number];
        }
    }
}