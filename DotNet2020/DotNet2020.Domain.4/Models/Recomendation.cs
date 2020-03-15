using System;
namespace DotNet2020.Domain.Models
{
    public class Recomendation
    {
        public int Id { get; }
        public int UserId { get; private set; }
        public string Text { get; private set; }

        public void SetRecomendationText(string text)
        {
            Text = text;
        }
    }
}
