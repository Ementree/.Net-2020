﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._1
{
    class WarningsExample
    {
        void LinqChainAndLineLength()
        {
            var col = new List<int>() { 1, 2 };
            col.Where(x => x > 0).Select(x => x * x).ToList();
            Console.WriteLine("11111111111111111111111111111111111111111111111111111111");
        }
        protected int PropertyEnc { get; private set; }

        void MethodParams(int nq, int n2)
        {

        }
    }
}