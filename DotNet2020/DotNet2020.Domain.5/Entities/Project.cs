using System;
using System.Collections.Generic;

namespace DotNet2020.Domain._5.Entities
{
    public class Project
    {
        public string Name { get; private set; }
        public List<User> Members { get; }

        public Project(string name, List<User> members)
        {
            if (members == null)
                throw new ArgumentException("Must be not null!", "Members");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Must be not empty!", "Name");

            Name = name;
            Members = members;
        }
    }
}
