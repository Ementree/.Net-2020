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
    /// Задача: Bool-свойства называйте с префиксом Is.
    /// https://kpfu-net.myjetbrains.com/youtrack/issue/1R-13
    /// </summary>
    public class BooleanPropsNameAnalyzer
    {
        public const string DiagnosticId = "BooleanPropsName";
        private const string Title = "Rename boolean property with Is prefix";
        private const string MessageFormat = "Rename boolean property with Is prefix";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var propertyDeclaration = context.Node as PropertyDeclarationSyntax;
            var predifinedType = propertyDeclaration
                .ChildNodes()
                .First(t =>
                    t.IsKind(SyntaxKind.PredefinedType)
                 );

            if (predifinedType != null)
            {
                var hasBool = predifinedType.ChildTokens().FirstOrDefault(t => t.IsKind(SyntaxKind.BoolKeyword));

                if (hasBool != null)
                {
                    var identifireToken = propertyDeclaration.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));

                    if (identifireToken.Value.ToString().IndexOf("Is") != 0)
                    {
                        var diagnostic = Diagnostic.Create(Rule, propertyDeclaration.GetLocation(), propertyDeclaration.GetText());
                        context.ReportDiagnostic(diagnostic);

                    }

                }

            }
        }

    }
}
