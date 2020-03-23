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
        public static List<string> GetQuestionsForCompetences(List<long> competencesId, CompetencesRepository competences)
        {
            List<string> questions=new List<string>();
            foreach (var element in competencesId)
            {
                var buf=competences.GetById(element).Questions;
                questions=questions.Union(buf).ToList();
            }
            return questions;
        }

        public static List<CompetencesModel> GetNamesOfChosenCompetences(List<long> competencesId, CompetencesRepository competences)
        {
            List<CompetencesModel> chosenCompetences=new List<CompetencesModel>();
            foreach (var element in competencesId)
            {
                var chosenCompetence=competences.GetById(element);
                chosenCompetences.Add(chosenCompetence);
            }
            return chosenCompetences;
        }

        public static void SaveAttestation(List<long> rightAnswers, List<long> skipedAnswers, List<string> commentaries, List<long> gotCompetences,
            List<string> questions, AttestationModel attestation, SpecificWorkerRepository workerRepository, AttestationRepository attestationRepository)
        {
            attestation.CompetencesId = gotCompetences;
            attestation.Date = DateTime.Today;
            attestationRepository.Create(attestation); //Создание аттестации
            AddCompetencesToWorker(workerRepository, gotCompetences, attestation);//добавить компетенции сотруднику
            var answersList = GetAnswersList(rightAnswers, skipedAnswers, commentaries, questions); //создать ответы
            attestationRepository.AddAnswersToAttestation(attestation, answersList); //добавить ответы и связать с аттестацией 
        }

        private static void AddCompetencesToWorker(SpecificWorkerRepository workerRepository, List<long> gotCompetences, AttestationModel attestation)
        {
            var worker = workerRepository.GetById(attestation.WorkerId.Value);
            workerRepository.SaveUpdateTable(worker, gotCompetences);
        }

        private static List<AnswerModel> GetAnswersList(List<long> rightAnswers, List<long> skipedAnswers, List<string> commentaries, List<string> questions)
        {
            List<AnswerModel> list=new List<AnswerModel>();

            for (int i = 0; i < questions.Count; i++)
            {
                if (commentaries[i] == null)
                    commentaries[i] = "Комментарий отсутствует!";
                list.Add(new AnswerModel{Commentary = commentaries[i], IsRight = rightAnswers.Contains(i), NumberOfAsk = i+1, IsSkipped = skipedAnswers.Contains(i)});
            }
            
            return list;
        }
    }
}