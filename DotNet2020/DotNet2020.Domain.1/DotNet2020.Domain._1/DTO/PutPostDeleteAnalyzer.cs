using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Rename;
using System.Threading;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// DTO для методов PUT/DELETE (команды) - по названию команды
    /// </summary>
    internal class PutPostDeleteAnalyzer
    {
        public const string DiagnosticId = "PutId";
        public const string CodeFixTitle = "Fix put method name";
        private const string Title = "Put/Delete warning";
        private const string MessageFormat = "shouldn't use DTO prefix in DTO class";
        private const string Category = "DTO";
        public static DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
                Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        internal static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var method = context.Node as MethodDeclarationSyntax;

            var attributes = method
                .ChildNodes()
                .FirstOrDefault(n => n.IsKind(SyntaxKind.AttributeList));

            if (attributes == null || attributes == default) return;

            var attributesTokens = ((AttributeListSyntax)attributes)
                .DescendantTokens()
                .Where(t => t.IsKind(SyntaxKind.IdentifierToken));

            if (!CheckAttribute(attributesTokens)) return;

            var paramsNames = ((ParameterListSyntax)method
                .ChildNodes()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.ParameterList)))
                .DescendantNodes()
                .Where(p => p.IsKind(SyntaxKind.IdentifierName))
                .Select(p => (IdentifierNameSyntax)p);

            if (paramsNames == null || paramsNames.Count() == 0) return;

            foreach(var p in paramsNames)
            {
                var t = ((SyntaxToken)p.ChildTokens().FirstOrDefault()).Text;
                if(new String(t.Skip(t.Length - 3).ToArray()).ToLower() == "dto")
                    context.ReportDiagnostic(Diagnostic.Create(Rule, p.GetLocation()));
            }
        }

        private static bool CheckAttribute(IEnumerable<SyntaxToken> attributes)
        {
            var attribute = attributes
                .FirstOrDefault(t => t.Text == "HttpDelete" || t.Text == "HttpPut" || t.Text == "HttpPost");
            return attribute != null && attribute != default;
        }

        public static async Task<Solution> 
            CodeFix(Document document, CodeFixContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
