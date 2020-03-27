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

namespace DotNet2020.Domain._1
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DotNet2020Domain_1CodeFixProvider)), Shared]
    public class DotNet2020Domain_1CodeFixProvider : CodeFixProvider
    {
        private const string title = "Encapsulate property";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DotNet2020Domain_1Analyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedSolution: c => EncapsulatePropertyAsync(context.Document, context, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Solution> EncapsulatePropertyAsync(Document document, CodeFixContext context,
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