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
    internal class PutDeleteAnalyzer
    {
        public const string DiagnosticPutId = "PutId";
        public const string DiagnosticDeleteId = "DeleteId";
        public const string CodeFixPutTitle = "Fix put method name";
        public const string CodeFixDeleteTitle = "Fix delete method name";
        private const string Title = "Put/Delete warning";
        private const string DeleteMessageFormat =
            "Delete method name should start with 'Delete'";
        private const string PutMessageFormat =
            "Put method name should start with 'Put'";
        private const string Category = "DTO";
        public static DiagnosticDescriptor DeleteRule =
            new DiagnosticDescriptor(DiagnosticDeleteId, Title, DeleteMessageFormat,
                Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        public static DiagnosticDescriptor PutRule =
            new DiagnosticDescriptor(DiagnosticPutId, Title, PutMessageFormat,
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

        public static async Task<Solution> PutCodeFix(Document document, 
            CodeFixContext context, CancellationToken cancellationToken)
        {
            return await CodeFix(document, context, "Put", cancellationToken).ConfigureAwait(false);
        }

        public static async Task<Solution> DeleteCodeFix(Document document,
            CodeFixContext context, CancellationToken cancellationToken)
        {
            return await CodeFix(document, context, "Delete", cancellationToken).ConfigureAwait(false);
        }

        private static async Task<Solution> CodeFix(Document document, CodeFixContext context, 
            string text, CancellationToken cancellationToken)
        {
            var root = await context
                            .Document
                            .GetSyntaxRootAsync(context.CancellationToken)
                            .ConfigureAwait(false);

            var diagnostic = context
                .Diagnostics
                .First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var localDecl = (MethodDeclarationSyntax)root.FindNode(diagnosticSpan);

            var identifierTokenName = localDecl.Identifier.Text;

            var newName = text + char.ToUpperInvariant(identifierTokenName[0])
                               + identifierTokenName.Substring(1, identifierTokenName.Length - 1);

            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(localDecl, cancellationToken);

            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution =
                await Renamer
                .RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken)
                .ConfigureAwait(false);

            return newSolution;
        }
    }
}
