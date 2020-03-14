using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class GradesModels
    {
        [Key]
        public long Id { get; set; }
        
        public string Grade { get; set; }
        
        public string[] Competences { get; set; }
    }
}