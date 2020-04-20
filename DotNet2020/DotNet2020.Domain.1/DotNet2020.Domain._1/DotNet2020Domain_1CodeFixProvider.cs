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
        private const string title2 = "add line break after attribute";

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

            context.RegisterCodeFix(
                CodeAction.Create(
                    title2,
                    c => OneAttributePerLine(context.Document, context, c),
                    title2),
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

        private async Task<Solution> OneAttributePerLine(Document document, CodeFixContext context,
            CancellationToken cancellationToken)
        {
            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            //var classDeclaration = root.FindNode(diagnosticSpan) as ClassDeclarationSyntax;
            var attributeList = root.FindNode(diagnosticSpan) as AttributeListSyntax;

            var modified = attributeList.CloseBracketToken.TrailingTrivia.Add(SyntaxFactory.EndOfLine(""));
            var modifiedBracket = attributeList.CloseBracketToken.WithTrailingTrivia(modified);
            var newRoot = root.ReplaceToken(attributeList.CloseBracketToken, modifiedBracket);
            return document.WithSyntaxRoot(newRoot).Project.Solution;
        }
    }
}