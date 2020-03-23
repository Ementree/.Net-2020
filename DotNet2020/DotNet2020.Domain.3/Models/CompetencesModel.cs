using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain._3.Models
{
    public class CompetencesModel
    {
        [Key]
        public long Id { get; set; }
        
        public string Competence { get; set; }
        
        public string[] Content { get; set; }
        
        public string[] Questions { get; set; }
        
        public List<GradeCompetencesModel> GradesCompetences { get; set; }
        public List<SpecificWorkerCompetencesModel> SpecificWorkerCompetencesModels { get; set; }
        
    }
}