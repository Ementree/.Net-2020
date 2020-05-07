using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._3.Models
{
    public enum CompetenceActions
    {
        AddContent,
        RemoveCompetence,
        RemoveContent
    }

    public class CompetenceUpdateModel
    {
        public CompetencesModel Competence { get; set; }
        public CompetenceActions Action { get; set; }
        public string Content { get; set; }
        public List<int> Checkboxes { get; set; } = new List<int>();
    }
}
