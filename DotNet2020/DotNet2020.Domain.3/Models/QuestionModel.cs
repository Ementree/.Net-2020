namespace DotNet2020.Domain._3.Models
{
    public class QuestionModel
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public QuestionComplexityModel Complexity { get; set; }
    }
}
