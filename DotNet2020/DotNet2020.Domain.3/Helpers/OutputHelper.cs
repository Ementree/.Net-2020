using System;
using System.Collections.Generic;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Helpers
{
    public class OutputHelper
    {
        public string WorkerName { get; set; }
        public List<string> Competences { get; set; }
        public DateTime Date { get; set; }
        public long AttestationId { get; set; }

        public OutputHelper(CompetencesRepository competences, SpecificWorkerRepository workers, AttestationModel attestationModel)
        {
            Date = attestationModel.Date;
            AttestationId = attestationModel.Id;
            var worker = workers.GetById((long) attestationModel.WorkerId);
            if (worker != null)
                WorkerName = worker.Name;
            else
                WorkerName = "Данный сотрудник был удалён!";
            Competences = new List<string>();
            foreach (var element in attestationModel.CompetencesId)
            {
                var competence = competences.GetById(element);
                if(competence!=null)
                    Competences.Add(competence.Competence);
                else
                    Competences.Add("Компетенция была удалена!");
            }
        }
    }
}