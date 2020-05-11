using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain._3.Models
{
    public class GradesModel
    {
        [Key]
        public long Id { get; set; }

        public string Grade { get; set; }

        public List<GradeCompetencesModel> GradesCompetences { get; set; } = new List<GradeCompetencesModel>();
    }
}