using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace DotNet2020.Domain._1
{

    /// <summary>
    /// При использовании Select в маппинге к связанной коллекции нужно всегда добавлять ToList.
    /// Это лечит n + 1 в EF. 
    /// https://docs.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-2.1#better-column-ordering-in-initial-migration
    /// </summary>
    class ToSelectOptimization
    {
        public const string DiagnosticId = "toSelectOptimization";
        public const string CodeFixTitle = "CodeFileTitle";
        private const string Title = "toSelect Optimization";
        private const string MessageFormat = "toSelect Optimization";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
           MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        private static bool GetIsSelectInsideExpression(SyntaxNode node)
        {
            return node.DescendantTokens().Any(token => token.IsKind(SyntaxKind.IdentifierToken) && token.ValueText == "Select");
        }


        private static bool GetSelectInArguments(InvocationExpressionSyntax ies)
        {
            var argumentList = ies.ChildNodes().Where(node => node.IsKind(SyntaxKind.ArgumentList));

            if (argumentList == null) return false;

            return GetIsSelectInsideExpression(argumentList.ToList()[0]);

        }


        private static SyntaxNode GetSimpleLambaExpression(SyntaxNode node)
        {
            return node.DescendantNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.SimpleLambdaExpression));
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = context.Node as InvocationExpressionSyntax;

            var isSelect = GetIsSelectInsideExpression(invocationExpression);
           
            if (isSelect)
            {
                var isSelectInArguments = GetSelectInArguments(invocationExpression);

                if(isSelectInArguments)
                {
                    var lambdaExpression = GetSimpleLambaExpression(invocationExpression);

                    var diagnostic = Diagnostic.Create(
                    Rule,
                    lambdaExpression.GetLocation(),
                    lambdaExpression.GetText()
                );

                context.ReportDiagnostic(diagnostic);

                }

            }
        }

        public static async Task<Solution> CodeFix(Document document, CodeFixContext context,
           CancellationToken cancellationToken)
        {
            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var currentNode = (SimpleLambdaExpressionSyntax)root.FindNode(diagnosticSpan);

            var nextNode = currentNode;

            var newRoot = root.ReplaceNode(currentNode, nextNode);

            return document.WithSyntaxRoot(newRoot).Project.Solution;
        }

    }
}
