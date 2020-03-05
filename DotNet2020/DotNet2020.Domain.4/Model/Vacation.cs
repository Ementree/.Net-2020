using System;
namespace DotNet2020.Domain._4.Model
{
    public class Vacation
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsApproved { get; set; }
    }
}
