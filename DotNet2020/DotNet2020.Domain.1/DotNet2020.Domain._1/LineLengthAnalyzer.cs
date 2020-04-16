using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// анализатор для проверки длины строк
    /// </summary>
    class LineLengthAnalyzer
    {
        public const string DiagnosticId = "LineLengthDiagnosticId";
        public const string CodeFixTitle = "make the line shorter";
        const int MaxLength = 80;
        const string Title = "line is too long";
        //static string mesFormat = $"line should be shorter than {MaxLength} symbols";
        const string MessageFormat = "line should be shorter than 80 symbols";
        const string Category = "Formatting";
        const string Description = "line should be shorter than 80 symbols";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var node = context.Node;
            var trivia = node.GetTrailingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            if (trivia != null &&
                (trivia.SpanStart - node.SpanStart) < MaxLength)
            {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(
                    new DiagnosticDescriptor(
                        DiagnosticId,
                        Title,
                        MessageFormat,
                        Category,
                        DiagnosticSeverity.Warning,
                        true,
                        description: Description),
                    node.GetLocation(),
                    node.GetText()));
        }

        public async Task<Solution> ChangeSolution()
        {
            throw new NotImplementedException();
        }
    }
}