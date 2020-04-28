using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Core.Models
{
    public class HasNameBase: HasIdBase
    {
        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}