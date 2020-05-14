using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public class AttestationListModel
    {
        public AttestationModel Attestation { get; set; }
        public SpecificWorkerModel Worker { get; set; }
        public List<CompetencesModel> Competences { get; set; }
        public AttestationListModel(AttestationModel attestation)
        {
            Attestation = attestation;
        }
    }
}
