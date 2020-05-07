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
        const string Category = "Stylistic";
        const string Description = "line should be shorter than 80 symbols";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void AnalyzeTree(SyntaxTreeAnalysisContext context)
        {
            var root = context.Tree.GetRoot();
            var tree = context.Tree;

            var lines = tree.GetText().Lines;
            foreach (var l in lines)
            {
                var span = l.Span;
                if (span.Length > MaxLength)
                    context.ReportDiagnostic(Diagnostic.Create(Rule, Location.Create(tree, span), l.Text));
            }
        }

        public async Task<Solution> ChangeSolution()
        {
            throw new NotImplementedException();
        }
    }
}