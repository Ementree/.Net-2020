using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class QuestionsModel //реализовать репозиторий
    {
        [Key]
        public long Id { get; set; }
        
        public string Competence { get; set; }
        
        public string[] Questions { get; set; }
    }
}