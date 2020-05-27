using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
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

        /// <summary>
        ///  Ищем запрос к БД db.Entity.Select/Where
        /// </summary>
        /// <param name="semanticModel"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private static bool GetIsSelectInsideExpression(SemanticModel semanticModel, SyntaxNode node)
        {
            return node.DescendantNodes().Any(subNode =>
            {
                var symbol = semanticModel.GetSymbolInfo(subNode);
                return symbol.Symbol != null && symbol.Symbol.ContainingSymbol.ToString() == "System.Linq.Queryable"
                     && (symbol.Symbol.Name == "Select" || symbol.Symbol.Name == "Where" || symbol.Symbol.Name == "FirstOrDefault");
            });
        }

        private static SyntaxNode CheckCondition(InvocationExpressionSyntax ies)
        {
            var argumentList = ies.ChildNodes().FirstOrDefault(node => node.IsKind(SyntaxKind.ArgumentList));
            if (argumentList == null) return null;

            return argumentList
                .DescendantNodes()
                .Where(node => node.IsKind(SyntaxKind.SimpleMemberAccessExpression) && node.TryGetInferredMemberName() == "Select")
                .FirstOrDefault(node =>
                        {
                            var parent = GetParentSimpleMemberAccessExpression(node);
                            return !(parent != null
                                    && parent.IsKind(SyntaxKind.SimpleMemberAccessExpression)
                                    && parent.TryGetInferredMemberName() == "ToList");
                        });
        }

        /// <summary>
        /// Get first parent of SimpleMemberAccessExpression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static SyntaxNode GetParentSimpleMemberAccessExpression(SyntaxNode node)
        {
            return node.Parent.Parent;
        }

        private static SyntaxNode GetSimpleLambaExpression(SyntaxNode node)
        {
            return node.DescendantNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.SimpleLambdaExpression));
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var invocationExpression = context.Node as InvocationExpressionSyntax;
            var isDbRequest = GetIsSelectInsideExpression(semanticModel, invocationExpression);

            if (isDbRequest)
            {
                var expressionError = CheckCondition(invocationExpression);

                if (expressionError != null)
                {
                    var token = expressionError.DescendantTokens()
                        .FirstOrDefault(t => t.ValueText == "Select" || t.ValueText == "Where");

                    //var argList = expressionError
                    //                 .ChildNodes().FirstOrDefault(node => node.IsKind(SyntaxKin));


                    var diagnostic = Diagnostic.Create(
                        Rule,
                        token.GetLocation(),
                        token.Text
                    //expressionError.GetText() 
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