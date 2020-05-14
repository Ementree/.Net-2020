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

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// DTO для методов PUT/DELETE (команды) - по названию команды
    /// </summary>
    internal class PutDeleteAnalyzer
    {
        public const string DiagnosticId = "PutDeleteId";
        public const string CodeFixTitle = "Put/Delete warning";
        private const string Title = "Put/Delete warning";
        private const string DeleteMessageFormat =
            "Delete method name should start with 'Delete'";
        private const string PutMessageFormat =
            "Put method name should start with 'Put'";
        private const string Category = "DTO";
        public static DiagnosticDescriptor DeleteRule =
            new DiagnosticDescriptor(DiagnosticId, Title, DeleteMessageFormat,
                Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        public static DiagnosticDescriptor PutRule =
            new DiagnosticDescriptor(DiagnosticId, Title, PutMessageFormat,
                Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        internal static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var method = context.Node as MethodDeclarationSyntax;

            var attributes = ((AttributeListSyntax)method
                .ChildNodes()
                .FirstOrDefault(n => n.IsKind(SyntaxKind.AttributeList)))
                .DescendantTokens()
                .Where(t => t.IsKind(SyntaxKind.IdentifierToken));

            var delete = CheckAttribute(attributes, "HttpDelete");
            var put = false;
            if (!delete) put = CheckAttribute(attributes, "HttpPut");

            var methodName = method
                .ChildTokens()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));

            if (delete) context = Report(context, methodName, "Delete", DeleteRule);
            else if (put) context = Report(context, methodName, "Put", PutRule);
        }

        private static SyntaxNodeAnalysisContext Report(SyntaxNodeAnalysisContext context, 
            SyntaxToken methodName, string text, DiagnosticDescriptor rule)
        {
            if (new String(methodName.Text.Take(text.Length).ToArray()) != text)
                context.ReportDiagnostic(
                Diagnostic.Create(rule, methodName.GetLocation()));
            return context;
        }

        private static bool CheckAttribute(IEnumerable<SyntaxToken> attributes, string text)
        {
            var attribute = attributes
                .FirstOrDefault(t => t.Text == text);
            return attribute != null && attribute != default;
        }

        public static async Task<Solution> ChangeSolution(Document document,
            CodeFixContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
