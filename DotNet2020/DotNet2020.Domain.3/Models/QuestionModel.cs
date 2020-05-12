using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public class QuestionModel
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public enum QuestionComplexity
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        }
    }
}
