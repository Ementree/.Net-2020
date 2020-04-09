using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class AnswerModel
    {
        [Key]
        public long AnswerId { get; set; }
        public int NumberOfAsk { get; set; }
        public bool IsSkipped { get; set; }
        public bool IsRight { get; set; }
        public string Commentary { get; set; }
        public string Question { get; set; }

        public List<AttestationAnswerModel> AttestationAnswer { get; set; }
    }
}