using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// Язык комментариев должен соответствовать договоренности на проекте:
    /// русский или английский
    /// </summary>
    public class LanguageAnalyzer
    {
        public const string DiagnosticId = "LanguageAnalyze";
        private const string Title = "Language comment's problem";
        private const string RussianMessageFormat = "Language should be Russian";
        private const string EnglishMessageFormat = "Language should be English";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor RussianRule = new DiagnosticDescriptor(DiagnosticId, Title,
            RussianMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        public static DiagnosticDescriptor EnglishRule = new DiagnosticDescriptor(DiagnosticId, Title,
            EnglishMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        //true - для русского языка, false - для английского
        public const bool IsRussian = true;

        public static void AnalyzeXML(SyntaxNodeAnalysisContext context)
        {
            var comment = context.Node as DocumentationCommentTriviaSyntax;

            var comments = comment
                .DescendantTokens()
                .Where(t => t.IsKind(SyntaxKind.XmlTextLiteralToken))
                .Select(t => new Comment(t));

            var commentContext = new CommentContext(context);
            Report(commentContext, comments);
        }

        public static void Analyze(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot();

            var comments = root.DescendantTrivia()
                .Where(t =>
                t.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                t.IsKind(SyntaxKind.MultiLineCommentTrivia))
                .Select(t =>
                {
                    var type = CommentType.None;
                    if (t.IsKind(SyntaxKind.SingleLineCommentTrivia)) type = CommentType.Single;
                    else if (t.IsKind(SyntaxKind.MultiLineCommentTrivia)) type = CommentType.Multi;
                    return new Comment(t, type);
                });

            var commentContext = new CommentContext(context);
            Report(commentContext, comments);
        }

        private static void Report(CommentContext context, IEnumerable<Comment> comments)
        {
            foreach (var c in comments)
            {
                var text = c.GetText;
                var location = c.GetLocation;
                if (IsRussian)
                {
                    if (text.IsNotRussian())
                        context.ReportDiagnostic(Diagnostic.Create(RussianRule, location));
                }
                else if (text.IsNotEnglish())
                    context.ReportDiagnostic(Diagnostic.Create(EnglishRule, location));
            }
        }
    }
}
