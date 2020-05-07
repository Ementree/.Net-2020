using Microsoft.CodeAnalysis;
using System.Linq;

namespace DotNet2020.Domain._1
{
    internal class Comment
    {
        internal Comment(SyntaxTrivia trivia, CommentType type)
        {
            this.trivia = trivia;
            Type = type;
        }

        internal Comment(SyntaxToken token)
        {
            this.token = token;
            Type = CommentType.Xml;
        }

        private readonly SyntaxTrivia trivia;
        private readonly SyntaxToken token;
        internal readonly CommentType Type;

        internal string GetText
        {
            get
            {
                if (Type == CommentType.Xml)
                    return token.ToString();
                var text = new string(trivia
                    .ToString()
                    .Skip(2)
                    .Where(c => c != 32)
                    .ToArray());
                if (Type.Equals(CommentType.Multi))
                    text = new string(text
                        .Take(text.Length - 2)
                        .ToArray());
                return text;
            }
        }

        internal Location GetLocation
        {
            get
            {
                if (Type == CommentType.Xml) return token.GetLocation();
                else return trivia.GetLocation();
            }
        }
    }

    internal enum CommentType { None, Single, Multi, Xml }
}
