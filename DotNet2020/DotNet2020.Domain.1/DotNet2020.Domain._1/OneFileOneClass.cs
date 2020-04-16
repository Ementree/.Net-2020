using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace DotNet2020.Domain._1
{
    /// <summary>
    /// Задача: Один класс - один файл
    /// https://kpfu-net.myjetbrains.com/youtrack/issue/1R-14
    /// </summary>
    public class OneFileOneClass
    {
        public const string DiagnosticId = "OneFileOneClass";
        private const string Title = "Use one class in one file";
        private const string MessageFormat = "Use one class in one file";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var namespaceDeclaration = context.Node as NamespaceDeclarationSyntax;

           if(namespaceDeclaration
              .ChildNodes()
              .Count(t => t.IsKind(SyntaxKind.ClassDeclaration)) > 1)
           {
                var firtsClass = namespaceDeclaration
                    .ChildNodes()
                    .First(n => n.IsKind(SyntaxKind.ClassDeclaration));

                if(firtsClass != null)
                {
                     var diagnostic = Diagnostic.Create(Rule, firtsClass.GetLocation(), firtsClass.GetText());
                     context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
