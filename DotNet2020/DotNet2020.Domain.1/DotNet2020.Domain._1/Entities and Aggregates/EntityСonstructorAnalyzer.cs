using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// Анализатор для проверки наличия конструктора с параметрами у сущностей
    /// </summary>
    class EntityСonstructorAnalyzer
    {
        public const string DiagnosticId = "EntityСonstructorDiagnosticId";
        public const string CodeFixTitle = "add constructor with params";
        const string Title = "Entity has not constructor";
        const string MessageFormat = "Entity should have a constructor with parameters";
        const string Category = "Entities and Aggregates";
        const string Description = "Entity should have a constructor with parameters";
        public static DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var classDecl = context.Node as ClassDeclarationSyntax;
            var isEntity = classDecl.AttributeLists
                .SelectMany(a => a.Attributes)
                .Select(a => a.Name.ToString())
                .Any(n => n == "Entity");

            var highlightArea = classDecl.Identifier;

            if (isEntity)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                highlightArea.GetLocation(),
                highlightArea.ValueText));
            }
        }
    }
}