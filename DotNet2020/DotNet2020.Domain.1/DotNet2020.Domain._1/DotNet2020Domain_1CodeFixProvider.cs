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
                    LineLengthAnalyzer.DiagnosticId,
                    FluentAnalyzer.DiagnosticId,
                    PropertyModifiersAnalyzer.DiagnosticId,
                    BooleanPropsNameAnalyzer.DiagnosticId,
                    MethodBodyAnalyze.DiagnosticId,
                    OneFileOneClass.DiagnosticId,
                    MethodParamsAnalyzer.DiagnosticId,
                    LanguageAnalyzer.DiagnosticId);
            }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            
        }
    }
}