using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// анализатор для проверки количества аргументов метода
    /// </summary>
    class MethodParamsAnalyzer
    {
        public const string DiagnosticId = "MethodParamsId";
        public const string CodeFixTitle = "Replace parameter list";
        const string Title = "Method has more than 1 parameter";
        const string MessageFormat = "Use Parameter Object or collection instead of paramer list";
        const string Category = "Design and refactoring";
        const string Description = "Use Parameter Object or collection instead of paramer list";
        const int ParamsCount = 1;
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var paramsList = (context.Node as MethodDeclarationSyntax).ParameterList;
            if (paramsList.Parameters.Count > ParamsCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                paramsList.GetLocation(), paramsList.GetText()));
            }
        }
    }
}