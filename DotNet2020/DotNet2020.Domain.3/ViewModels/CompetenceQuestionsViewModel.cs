using DotNet2020.Domain._3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._3.ViewModels
{
    public class CompetenceQuestionsViewModel
    {
        public CompetencesModel Competence { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public CompetenceQuestionsViewModel(CompetencesModel competence, IEnumerable<CompetenceQuestionsModel> competenceQuestions,
            IEnumerable<QuestionModel> questions)
        {
            Competence = competence;
            var questionIds = competenceQuestions.Where(x => x.CompetenceId == Competence.Id).Select(x => x.QuestionId).ToList();
            Questions = questions.Where(x => questionIds.Contains(x.Id)).ToList();
        }
    }
}
