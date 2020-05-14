using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// анализатор для проверки форматирования при использовании fluent-интерфейса =
    /// каждый метод с новой строки.
    /// </summary>
    class FluentAnalyzer
    {
        public const string DiagnosticId = "FluentDiagnosticId";
        public const string CodeFixTitle = "Put method on a new line";
        const string Title = "violation of fluent-chain convention";
        const string MessageFormat = "Fluent-chain convention: 1 method per 1 line";
        const string Description = "Fluent-chain convention: 1 method per 1 line";
        const string Category = "Stylistic";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, 
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var expression = context.Node as MemberAccessExpressionSyntax;
            var dotToken = expression
                .ChildTokens()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.DotToken));

            if (expression.ChildNodes().Any(n => n.IsKind(SyntaxKind.InvocationExpression)) &&
                dotToken != null &&
                !dotToken.LeadingTrivia.Any(t => t.IsKind(SyntaxKind.WhitespaceTrivia)))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                    expression.GetLocation(),
                    expression.GetText()));
            }
        }

        public async Task<Solution> CodeFix()
        {
            throw new NotImplementedException();
        }
    }
}