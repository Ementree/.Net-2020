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
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DotNet2020Domain_1Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PropertyDiagnosticId";
        public const string CodeFixTitle = "Change modificator";

        private const string Title = "Property encapsulation problem";
        private const string MessageFormat = @"Property should have public get and protected set";
        private const string Category = "Incapsulation";
        private const string Description = @"Property should have public get and protected set";
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.PropertyDeclaration);
        }

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var propertyNode = (PropertyDeclarationSyntax)context.Node;
            var setAccessor = propertyNode.AccessorList.Accessors
                .FirstOrDefault(acc => acc.IsKind(SyntaxKind.SetAccessorDeclaration));

            if (propertyNode.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)) &&
                (setAccessor is null || setAccessor.Modifiers.Any(m => m.IsKind(SyntaxKind.ProtectedKeyword))))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule,
                propertyNode.Identifier.GetLocation(),
                propertyNode.Identifier.Text));
        }
    }
}