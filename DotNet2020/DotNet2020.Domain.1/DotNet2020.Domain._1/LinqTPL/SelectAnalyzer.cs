using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Rename;
using System.Threading;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static System.Console;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// Не используйте $”{интерполяцию}” в Select и AutoMapper.
    /// Эти конструкции транслируются без .Where или .OrderBy, но падают с ними.
    /// </summary>
    internal class SelectAnalyzer
    {
        public const string DiagnosticId = "SelectDiagnosticId";
        public const string CodeFixTitle = "Select warning";
        private const string Title = "Select warning";
        private const string MessageFormat = 
            "Shouldn't use interpolation in Select";
        private const string Category = "Formatting";
        public static DiagnosticDescriptor Rule = 
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, 
                Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        internal static void Analyze (SyntaxNodeAnalysisContext context)
        {
            var expression = context.Node as MemberAccessExpressionSyntax;
            SyntaxToken selectToken = FindSelectToken(expression);

            if (selectToken == null || selectToken == default) return;
            
            var interpolations = FindInterpolations(selectToken);
            foreach (var i in interpolations)
                context.ReportDiagnostic(Diagnostic.Create(Rule, i.GetLocation()));
            
        }

        private static SyntaxToken FindSelectToken(MemberAccessExpressionSyntax expression)
        {
            return expression
                .ChildNodes()
                .Where(n => n.IsKind(SyntaxKind.IdentifierName))
                .Select(n => n
                    .ChildTokens()
                    .FirstOrDefault(t =>
                    t.IsKind(SyntaxKind.IdentifierToken) && t.Text == "Select"))
                .Where(t => t != default)
                .FirstOrDefault();
        }

        private static List<InterpolatedStringExpressionSyntax> FindInterpolations(SyntaxToken selectToken)
        {
            return selectToken
               .Parent
               .Parent
               .Parent
               .ChildNodes()
               .FirstOrDefault(n => n.IsKind(SyntaxKind.ArgumentList))
                .DescendantNodes()
                .Where(n => n.IsKind(SyntaxKind.InterpolatedStringExpression))
                .Select(i => (InterpolatedStringExpressionSyntax)i)
                .ToList();
        }

        public static async Task<Solution> CodeFix( Document document, 
            CodeFixContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
