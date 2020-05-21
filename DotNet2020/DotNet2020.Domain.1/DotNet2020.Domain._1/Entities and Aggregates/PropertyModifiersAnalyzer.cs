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
    /// анализатор для проверки инкапсуляции строк
    /// </summary>
    class PropertyModifiersAnalyzer
    {
        public const string DiagnosticId = "PropertyDiagnosticId";
        public const string CodeFixTitle = "Change modificator";
        private const string Title = "Property encapsulation problem";
        private const string MessageFormat = @"Property should have public get and protected set";
        private const string Description = @"Property should have public get and protected set";
        private const string Category = "Entities and Aggregates";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var propertyNode = (PropertyDeclarationSyntax)context.Node;

            var setAccessor = propertyNode.AccessorList.Accessors
                .FirstOrDefault(acc => acc.IsKind(SyntaxKind.SetAccessorDeclaration));

            if (propertyNode.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) &&
                (setAccessor is null || setAccessor.Modifiers.Any(m => m.IsKind(SyntaxKind.ProtectedKeyword))))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule,
                propertyNode.Identifier.GetLocation(),
                propertyNode.Identifier.Text));
        }

        public static async Task<Solution> CodeFix(Document document, CodeFixContext context,
            CancellationToken cancellationToken)
        {
            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var property = (PropertyDeclarationSyntax)root.FindNode(diagnosticSpan);

            var setAccessor = property.AccessorList.Accessors
                .FirstOrDefault(acc => acc.IsKind(SyntaxKind.SetAccessorDeclaration));
            var protectedModifier = SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
            var modifiedSetter = setAccessor
                .WithModifiers(SyntaxFactory.TokenList(protectedModifier));

            var modifiedProperty = property
                .ReplaceNode(setAccessor, modifiedSetter)
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

            var newRoot = root.ReplaceNode(property, modifiedProperty);

            return document.WithSyntaxRoot(newRoot).Project.Solution;
        }
    }
}