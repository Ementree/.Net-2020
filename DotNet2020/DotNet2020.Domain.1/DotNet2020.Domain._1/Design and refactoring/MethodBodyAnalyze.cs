using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace DotNet2020.Domain._1
{
    class MethodBodyAnalyze
    {
        public const string DiagnosticId = "MethodBodyAnalyze";
        private const string Title = "MethodBodyAnalyze";
        private const string MessageFormat = "MethodBodyAnalyze";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        private static int totalCount = 0;

        private static void GetContInDeep(SyntaxNode node, SyntaxKind syntaxKind)
        {

            node.ChildNodes().All(t =>
            {
                GetContInDeep(t, syntaxKind);
                return true;
            });

            node.ChildTokens().All(t =>
            {
                var count = t
                .GetAllTrivia()
                .Count(t2 => t2.IsKind(SyntaxKind.EndOfLineTrivia));

                totalCount += count;

                return true;
            });
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node as MethodDeclarationSyntax;

            var methodBlock = methodDeclaration.ChildNodes().First(n => n.IsKind(SyntaxKind.Block));
            //if (methodBlock != null) return;


            if (methodBlock == null) return;

            totalCount = 0;


            GetContInDeep(methodBlock, SyntaxKind.EndOfLineTrivia);

            if (totalCount > 20)
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    methodDeclaration.GetLocation(),
                    methodDeclaration.GetText(),
                    totalCount
                    );

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}