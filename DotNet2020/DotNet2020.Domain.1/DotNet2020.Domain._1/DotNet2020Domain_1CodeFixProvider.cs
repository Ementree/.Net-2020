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
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                return ImmutableArray.Create(
                    PropertyModifiersAnalyzer.DiagnosticId,
                    PutPostDeleteAnalyzer.DiagnosticId,
                    SelectAnalyzer.DiagnosticId);
            }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            switch (diagnostic.Id)
            {
                case PropertyModifiersAnalyzer.DiagnosticId:
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: PropertyModifiersAnalyzer.CodeFixTitle,
                            createChangedSolution: c => PropertyModifiersAnalyzer.CodeFix(context.Document, context, c),
                            equivalenceKey: PropertyModifiersAnalyzer.CodeFixTitle),
                        diagnostic);
                    break;
                case PutPostDeleteAnalyzer.DiagnosticId:
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: PutPostDeleteAnalyzer.CodeFixTitle,
                            createChangedSolution: c => PutPostDeleteAnalyzer.CodeFix(context.Document, context, c),
                            equivalenceKey: PutPostDeleteAnalyzer.CodeFixTitle),
                        diagnostic);
                    break;
                case SelectAnalyzer.DiagnosticId:
                    context.RegisterCodeFix(
                        CodeAction.Create(
                            title: SelectAnalyzer.CodeFixTitle,
                            createChangedSolution: c => SelectAnalyzer.CodeFix(context.Document, context, c),
                            equivalenceKey: SelectAnalyzer.CodeFixTitle),
                        diagnostic);
                    break;
            }
        }
    }
}