using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._4.Model
{
    public class Absence
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Reason Reason { get; set; }
    }

    public enum Reason
    {
        Illness,
        Seekday,
    }
}
