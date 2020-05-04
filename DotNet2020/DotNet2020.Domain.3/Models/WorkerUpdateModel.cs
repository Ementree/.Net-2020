using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public class WorkerUpdateModel
    {
        public SpecificWorkerModel Worker { get; set; }
        public List<long> NewCompetencesIds { get; set; } = new List<long>();
        public List<CompetencesModel> Competences { get; set; } = new List<CompetencesModel>();

    }
}
