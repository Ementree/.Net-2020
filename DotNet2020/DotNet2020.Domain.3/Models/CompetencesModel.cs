using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class CompetencesModel
    {
        [Key]
        public long Id { get; set; }
        
        public string Competence { get; set; }
        
        public string[] Content { get; set; }
    }
}