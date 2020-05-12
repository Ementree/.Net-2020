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
 
         public List<string> Content { get; set; } = new List<string>();
         
         public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
         public List<GradeCompetencesModel> GradesCompetences { get; set; } = new List<GradeCompetencesModel>();
         public List<SpecificWorkerCompetencesModel> SpecificWorkerCompetencesModels { get; set; } = new List<SpecificWorkerCompetencesModel>();
 
     }
 }