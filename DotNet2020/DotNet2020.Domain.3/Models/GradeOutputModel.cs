using System.Collections.Generic;
using DotNet2020.Domain._3.Repository;

namespace DotNet2020.Domain._3.Models
{
    public static class GradeOutputModelHelper
    {
        public static List<GradeOutputModel> GetList(GradesRepository gradesRepository)
        {
            var list=new List<GradeOutputModel>();
            foreach (var grade in gradesRepository.GetList())
            {
                var buf = gradesRepository.GetAllCompetencesById(grade.Id);
                list.Add(new GradeOutputModel {Grade = grade, Competences = buf});
            }
            return list;
        }
    }

    public class GradeOutputModel
    {
        public GradesModel Grade { get; set; }
        public List<CompetencesModel> Competences { get; set; }
    }
}