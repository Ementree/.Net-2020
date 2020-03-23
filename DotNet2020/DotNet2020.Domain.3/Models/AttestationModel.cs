using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain._3.Models
{
    public class AttestationModel
    {
        [Key]
        public long Id { get; set; }
        public long? WorkerId { get; set; }
        public List<long> CompetencesId { get; set; }
        public string Problems { get; set; }
        public string NextMoves { get; set; }
        public string Feedback { get; set; }
        public DateTime Date { get; set; }
        public List<AttestationAnswerModel> AttestationAnswer { get; set; }
    }
}