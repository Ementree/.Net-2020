using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public enum QuestionActions
    {
        RemoveQuestions,
        AddQuestion
    }

    public class QuestionUpdateModel
    {
        public List<string> Questions { get; set; }
        public QuestionActions Action { get; set; }
        public string NewQuestion { get; set; }
        public List<string> QuestionsToRemove { get; set; } = new List<string>();
    }
}
