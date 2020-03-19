using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Public set";
        private const string Title = "Property encapsulation problem";
        private const string MessageFormat = "Property's '{0}' setter is public or getter is unpublic";
        private const string Category = "Syntax";
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Property);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var propertySymbol = (IPropertySymbol)context.Symbol;

            if (propertySymbol != null &&
                (propertySymbol.GetMethod.DeclaredAccessibility != Accessibility.Public ||
                propertySymbol.SetMethod.DeclaredAccessibility != Accessibility.Protected))
            {
                var diagnostic = Diagnostic.Create(Rule, propertySymbol.Locations[0], propertySymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}