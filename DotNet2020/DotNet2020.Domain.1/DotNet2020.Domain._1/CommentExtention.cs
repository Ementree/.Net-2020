using System.Linq;
using Microsoft.CodeAnalysis;

namespace DotNet2020.Domain._1
{
    internal static class CommentExtention
    {
        internal static bool IsRussian(this char c)
        {
            return c >= 1040 && c <= 1103 || c == 1025 || c == 1105;
        }

        internal static bool IsNotEnglish(this string str)
        {
            return str.Any(c => c.IsRussian());
        }

        internal static bool IsNotRussian(this string str)
        {
            var russianSymbCount = str
                .Where(c => c.IsRussian())
                .Count();
            return 2 * russianSymbCount <= str.Length;
        }
    }
}
