using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DotNet2020.Domain.Core.Models;

namespace DotNet2020.Domain._3.Models
{
    public class SpecificWorkerModel : Employee
    {
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public string Commentary { get; set; }
        public string PreviousWorkPlaces { get; set; }
        public string Experience { get; set; }
        [NotMapped]
        public string FullName => LastName + " " + FirstName + " " + MiddleName;

        public string Initials => LastName + " " + FirstName[0] + "." + " " + MiddleName[0] +".";

        public List<SpecificWorkerCompetencesModel> SpecificWorkerCompetencesModels { get; set; } = new List<SpecificWorkerCompetencesModel>();
    }
}