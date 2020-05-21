using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._1
{
    class WarningsExample
    {
        /// <summary>
        /// какой-то комментарий
        /// some comment
        /// комментарий with latin and cyrillic
        /// comment с латиницей и кириллицей
        /// </summary>
        void LinqChainAndLineLength()
        {
            var col = new List<int>() { 1, 2 };
            col.Where(x => x > 0).Select(x => x * x).ToList();
            Console.WriteLine("11111111111111111111111111111111111111111111111111111111");
            var q = new Queue<int>()
                .Select(i => $"some");
        }
        protected int PropertyEnc { get; private set; }

        [HttpPost]
        public IActionResult Post(UpdateInfoDto command)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Post(UpdateSomeInfo command)
        {
            throw new NotImplementedException();
        }

        void MethodParams(int nq, int n2)
        {

        }
    }
}
