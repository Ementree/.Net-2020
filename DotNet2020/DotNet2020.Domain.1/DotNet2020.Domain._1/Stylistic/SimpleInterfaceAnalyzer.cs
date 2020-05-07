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
    /// Задача: Маленькие интерфейсы (интерфейс только с одним свойством) со свойствами называйте с префиксом IHas.
    /// https://kpfu-net.myjetbrains.com/youtrack/issue/1R-13
    /// </summary>
    public class SimpleInterfaceAnalizer
    {
        private const string DiagnosticId = "SimpleInterface";
        private const string Title = "Rename interface with IHas prefix";
        private const string MessageFormat = "Rename interface with IHas prefix";
        private const string Category = "Syntax";

        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var interfaceDeclaration = context.Node as InterfaceDeclarationSyntax;

            if (interfaceDeclaration
                .ChildNodes()
                .Count(item => item.IsKind(SyntaxKind.PropertyDeclaration)) == 1)
            {
                var interfaceName = interfaceDeclaration
                    .ChildTokens()
                    .First(t => t.IsKind(SyntaxKind.IdentifierToken));
                if (interfaceName.ToString().IndexOf("IHas") != 0)
                {
                    var diagnostic = Diagnostic.Create(Rule, interfaceDeclaration.GetLocation(), interfaceDeclaration.GetText());
                    context.ReportDiagnostic(diagnostic);
                }
            }

        }

    }
}
