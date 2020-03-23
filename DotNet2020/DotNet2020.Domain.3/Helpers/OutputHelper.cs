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

        public OutputHelper(CompetencesRepository competences, SpecificWorkerRepository workers, AttestationModel attestationModel)
        {
            Date = attestationModel.Date;
            WorkerName = workers.GetById((long)attestationModel.WorkerId).Name;
            Competences = new List<string>();
            foreach (var element in attestationModel.CompetencesId)
            {
                Competences.Add(competences.GetById(element).Competence);
            }
        }
    }
}