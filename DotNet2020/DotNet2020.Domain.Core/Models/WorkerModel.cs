using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet2020.Domain.Core.Models
{
    public class WorkerModel
    {
       [Key]
       public long Id { get; set; }
       public string Name { get; set; }
       public string Surname { get; set; }
       public string Patronymic { get; set; }
       public string Position { get; set; }
    }
}