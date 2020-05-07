using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public enum GradeActions
    {
        SetCompetences,
        RemoveGrade,
        AddGrade
    }


    public class GradeUpdateModel
    {
        public GradeActions Action { get; set; }
        public GradesModel GradeModel { get; set; }
        public List<CompetencesModel> Competences { get; set; }
        public List<GradesModel> Grades { get; set; }

        public List<long> NewCompetencesIds { get; set; } = new List<long>();
        public List<long> NewGradesIds { get; set; } = new List<long>();
        public List<GradeToGradeModel> GradeToGrades { get; set; }

        public List<CompetencesModel> OldCompetences { get; set; }
    }
}
