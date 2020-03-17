using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._4.Models
{
    public class Recommendation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите текст рекомендации")]
        public string RecommendationText { get; set; }
    }
}
