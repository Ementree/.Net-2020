using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotNet2020.Domain.Core.Models;

namespace DotNet2020.Domain._3.Models
{
    public class SpecificWorkerModel : WorkerModel
    {
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public string Commentary { get; set; }
        public string PreviousWorkPlaces { get; set; }
        public string Experience { get; set; }
        
        public List<SpecificWorkerCompetencesModel> SpecificWorkerCompetencesModels { get; set; }
    }
}