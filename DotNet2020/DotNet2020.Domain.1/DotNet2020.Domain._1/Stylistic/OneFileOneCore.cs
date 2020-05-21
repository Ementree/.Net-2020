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
    /// Давайте захватим интерфейсы и enum(ы). 
    /// Т.е анализ не только для классов, но и для интерфейсов и enum(ов). 
    /// Т.е если эти типы находятся в файле не одни, 
    /// то подчеркивать название типа (класса, енама или интерфейса) и 
    /// предлагать перенести в другой файл. 
    /// Т.е в файле всегда либо один класс, либо один интерфейс, либо один енам
    /// </summary>
    public class OneFileOneCore
    {
        private const string DiagnosticId = "OneFileOneCore";
        private const string Title = "Move to single file";
        private const string MessageFormat = "Move to single file";
        private const string Category = "Syntax";

        private static SyntaxKind[] kinds = {
                SyntaxKind.ClassDeclaration,
                SyntaxKind.InterfaceDeclaration,
                SyntaxKind.EnumDeclaration
         };

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        private static bool CheckKind(NamespaceDeclarationSyntax namespaceDeclaration, SyntaxKind kind)
        {
            return (namespaceDeclaration
              .ChildNodes()
              .Count(t => t.IsKind(kind)) > 1);
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node)
        {
            var diagnostic = Diagnostic.Create(Rule, node.GetLocation(), node.GetText());
            context.ReportDiagnostic(diagnostic);
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var namespaceDeclaration = context.Node as NamespaceDeclarationSyntax;

            kinds.ToList().ForEach(kind =>
            {
                if(CheckKind(namespaceDeclaration, kind))
                {
                    var nodes = namespaceDeclaration.ChildNodes().Where(node => node.IsKind(kind));
                    nodes.ToList().ForEach(node => ReportDiagnostic(context, node));
                }
            });
        }
    }
}
