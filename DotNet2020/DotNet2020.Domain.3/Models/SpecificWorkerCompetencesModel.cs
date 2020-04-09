using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class SpecificWorkerCompetencesModel
    {
        public long WorkerId { get; set; }
        public SpecificWorkerModel Worker { get; set; }
        public long CompetenceId { get; set; }
        public CompetencesModel Competence { get; set; }
    }
}