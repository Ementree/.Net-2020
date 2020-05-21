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
    ///  Если енамы или классы находятся внутри других классов - то предлагать их заприватить.
    /// </summary>
    class NestedEnumAndClass
    {

        private const string DiagnosticId = "PrivateEnumAndClass";
        private const string Title = "Make Class Enum/side Class private";
        private const string MessageFormat = "Make Class inake Enum/side Class private";
        private const string Category = "Syntax";

        private static SyntaxKind[] kinds = {
                SyntaxKind.ClassDeclaration,
                SyntaxKind.EnumDeclaration
         };

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        private static List<SyntaxNode> GetNodes(ClassDeclarationSyntax classDeclaration, SyntaxKind kind)
        { 
            return classDeclaration
              .ChildNodes()
              .Where(node => node.IsKind(kind) && !node.ChildTokens().Any(token => token.IsKind(SyntaxKind.PrivateKeyword))).ToList();
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node)
        {
            var diagnostic = Diagnostic.Create(Rule, node.GetLocation(), node.GetText());
            context.ReportDiagnostic(diagnostic);
        }

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = context.Node as ClassDeclarationSyntax;

            kinds.ToList().ForEach(kind =>
            {
                GetNodes(classDeclaration, kind).ForEach(node => ReportDiagnostic(context, node));
            });
        }
    }
}
