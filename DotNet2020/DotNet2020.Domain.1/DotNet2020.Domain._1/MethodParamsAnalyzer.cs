using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// анализатор для проверки количества аргументов метода
    /// </summary>
    class MethodParamsAnalyzer
    {
        public const string DiagnosticId= "MethodParamsId";
        public const string CodeFixTitle = "use Parameter Object or collection instead of paramer list";
        const string Title = "Replace parameter list";
        const string MessageFormat = "Use Parameter Object or collection instead of paramer list";
        const string Category = "Design and refactoring";
        const string Description = "Use Parameter Object or collection instead of paramer list";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            // cразу принимать на вход parameterlistsyntax?
            var paramsList = (context.Node as MethodDeclarationSyntax).ParameterList;
            if (paramsList.Parameters.Count > 1)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                paramsList.GetLocation(), paramsList.GetText()));
            }
        }

        public Task<Solution> ChangeSolution()
        {
            throw new NotImplementedException(); 
        }
    }
}