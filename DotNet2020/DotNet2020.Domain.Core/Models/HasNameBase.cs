using System.ComponentModel.DataAnnotations;

namespace DotNet2020.Domain.Core.Models
{
    public class HasNameBase : HasIdBase
    {
        [Required, StringLength(Strings.DefaultLength)]
        public string Name { get; set; }
    }
}