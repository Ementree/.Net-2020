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
    /// Доработать! Enum
    /// https://kpfu-net.myjetbrains.com/youtrack/issue/1R-14
    /// </summary>
    class EnumCheck
    {
        private const string DiagnosticId = "EnumCheck";
        private const string Title = "Move Enum to single file or change modificator to private";
        private const string MessageFormat = "Move Enum to single file or change modificator to private";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
           MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var enumDeclaration = context.Node as EnumDeclarationSyntax;
            

            if(enumDeclaration != null)
            {
                var parent = enumDeclaration.Parent;

                if (parent == null) return;

                var isClassDeclarationNear = parent
                    .ChildNodes()
                    .Any(n => n.IsKind(SyntaxKind.ClassDeclaration));

                if (!isClassDeclarationNear) return;

                var isPrivate = enumDeclaration
                    .ChildTokens()
                    .Any(t => t.IsKind(SyntaxKind.PrivateKeyword));

                if (isPrivate) return;

               var diagnostic = Diagnostic.Create(
                   Rule, 
                   enumDeclaration.GetLocation(), 
                   enumDeclaration.GetText()
                   );

               context.ReportDiagnostic(diagnostic);
            }

        }

    }
}
