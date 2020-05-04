using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain._3.Models
{
    public enum AttestationAction
    {
        None,
        Choosing,
        CompetencesChose,
        GradeChose,
        AttestationByCompetences,
        AttestationByGrade,
        Finished
    }
    public class AttestationModel
    {
        [Key]
        public long Id { get; set; }
        public long WorkerId { get; set; }
        public List<long> GotCompetences { get; set; }
        public string Problems { get; set; }
        public string NextMoves { get; set; }
        public string Feedback { get; set; }
        public DateTime Date { get; set; }
        
        public List<long> IdsTestedCompetences { get; set; }
        [NotMapped]
        public List<CompetencesModel> TestedCompetences { get; set; }
        [NotMapped]
        public List<AttestationAnswerModel> AttestationAnswer { get; set; } = new List<AttestationAnswerModel>();

        [NotMapped]
        public AttestationAction Action { get; set; }
        [NotMapped]
        public List<SpecificWorkerModel> Workers { get; set; }
        [NotMapped]
        public List<CompetencesModel> Competences { get; set; }
        [NotMapped]
        public List<GradesModel> Grades { get; set; }
        [NotMapped]
        public long? GradeId { get; set; }
        [NotMapped]
        public bool? IsGotGrade { get; set; }
        [NotMapped]
        public List<long> RightAnswers { get; set; } = new List<long>();
        [NotMapped]
        public List<long> SkipedAnswers { get; set; } = new List<long>();
        [NotMapped]
        public List<string> Commentaries { get; set; } = new List<string>();
        [NotMapped]
        public List<string> Questions { get; set; } = new List<string>();
    }
}