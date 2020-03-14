using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain._3.Models
{
    public class WorkerModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public double Bonus { get; set; }
        public string Commentary { get; set; }
        public string[] Competences { get; set; }
        public string PreviousWorkPlaces { get; set; }
        public string Experience { get; set; }
    }
}