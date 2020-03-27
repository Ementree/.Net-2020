﻿using DotNet2020.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._6.Models
{
    public class Period
    {
        public int Id { get; protected set; }
        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
    }
}