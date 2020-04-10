using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain._3.Models
{
    public class GradeCompetencesModel
    {
        public long GradeId { get; set; }
        public GradesModel Grade { get; set; }
        public long CompetenceId { get; set; }
        public CompetencesModel Competence { get; set; }
    }
}