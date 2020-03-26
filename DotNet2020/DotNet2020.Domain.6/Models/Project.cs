using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class Project
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int StatusId { get; protected set; }
    }
}
