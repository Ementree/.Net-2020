using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class ProjectStatus
    {
        public ProjectStatus(int id, string status)
        {
            Id = id;
            Status = status;
        }
        protected ProjectStatus() : base() { }
        public int Id { get; protected set; }
        public string Status { get; protected set; }
    }
}
