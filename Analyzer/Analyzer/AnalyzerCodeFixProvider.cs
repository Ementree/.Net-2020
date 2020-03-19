using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerCodeFixProvider)), Shared]
    public class AnalyzerCodeFixProvider : CodeFixProvider
    {
        private const string title = "Encapsulate property";
        public string Foo { get; private set; } = $"{title}";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerAnalyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start);

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title,
                    c => EncapsulatePropertyAsync(context.Document, declaration, c),
                    AnalyzerAnalyzer.DiagnosticId),
                diagnostic);
        }

        private async Task<Document> EncapsulatePropertyAsync(Document document, SyntaxToken declaration, CancellationToken cancellationToken)
        {
            var prop = FindAncestorOfType<PropertyDeclarationSyntax>(declaration.Parent);

            var propertyDeclaration = CreatePropertyDecaration(declaration.ValueText, prop.Type);

            var root = await document.GetSyntaxRootAsync();
            var newRoot = root.ReplaceNode(prop, propertyDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);

            return newDocument;
        }
        private T FindAncestorOfType<T>(SyntaxNode node) where T : SyntaxNode
        {
            if (node == null)
                return null;
            return node is T ? node as T : FindAncestorOfType<T>(node.Parent);
        }

        private PropertyDeclarationSyntax CreatePropertyDecaration(string propertyName, TypeSyntax propertyType)
        {
            return SyntaxFactory.PropertyDeclaration(propertyType, propertyName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.ProtectedKeyword)));
        }
    }
}