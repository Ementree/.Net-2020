using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public static class AttestationHelper
    {
        public static List<string> GetQuestionsForCompetences(List<long> competencesId,
            CompetencesRepository competences)
        {
            List<string> questions=new List<string>();
            foreach (var element in competencesId)
            {
                var buf=competences.GetById(element).Questions;
                questions=questions.Union(buf).ToList();
            }
            return questions;
        }

        public static List<CompetencesModel> GetNamesOfChosenCompetences(List<long> competencesId,
            CompetencesRepository competences)
        {
            List<CompetencesModel> chosenCompetences=new List<CompetencesModel>();
            foreach (var element in competencesId)
            {
                var chosenCompetence=competences.GetById(element);
                chosenCompetences.Add(chosenCompetence);
            }
            return chosenCompetences;
        }

        public static void SaveAttestation(List<long> rightAnswers, List<long> skipedAnswers, List<string> commentaries, AttestationRepository attestation, AttestationModel model,
            CompetencesRepository competences, List<long> gotCompetences, WorkerRepository workers)
        {
            var questions= GetQuestionsForCompetences(model.CompetencesId, competences);
            model.Answers = GetAnswers(rightAnswers, skipedAnswers, commentaries, questions);
            model.CompetencesId = gotCompetences;
            model.Date = DateTime.Today;
            attestation.Create(model);
            var worker = workers.GetById((long)model.WorkerId);
            List<string> gotCompetencesList=new List<string>();
            foreach (var competenceId in gotCompetences)
            {
                gotCompetencesList.Add(competences.GetById(competenceId).Competence);
            }
            worker.Competences = worker.Competences.Union(gotCompetencesList).ToArray();
            workers.Save();
        }

        private static string GetAnswers(List<long> rightAnswers, List<long> skipedAnswers, List<string> commentaries, List<string> questions)
        {
            List<BuilderHelper> list=new List<BuilderHelper>();
            for (int i = 0; i < questions.Count; i++)
            {
                var builder=new BuilderHelper();
                builder.NumberQuestion = i + 1;
                builder.Question = questions[i];
                if (rightAnswers.Contains(i))
                    builder.IsRight = true;
                if (skipedAnswers.Contains(i))
                    builder.Skip = true;
                if (commentaries[i] != null)
                    builder.Commentary = commentaries[i];
                list.Add(builder);
            }

            StringBuilder feedback = new StringBuilder("");

            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].Skip==false)
                    feedback.Append(list[i].ToString());
            }

            feedback = feedback.Remove(feedback.Length - 3, 3);
            return feedback.ToString();
        }
    }
}