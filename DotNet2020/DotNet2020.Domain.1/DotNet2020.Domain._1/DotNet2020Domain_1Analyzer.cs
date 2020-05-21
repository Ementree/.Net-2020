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
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    LineLengthAnalyzer.Rule,
                    FluentAnalyzer.Rule,
                    PropertyModifiersAnalyzer.Rule,
                    MethodParamsAnalyzer.Rule,
                    Entity—onstructorAnalyzer.Rule,
                    BooleanPropsNameAnalyzer.Rule,
                    MethodBodyAnalyze.Rule,
                    SimpleInterfaceAnalizer.Rule,
                    EnumCheck.Rule,
                    OneFileOneClass.Rule,
                    LanguageAnalyzer.RussianRule,
                    LanguageAnalyzer.EnglishRule,
                    SelectAnalyzer.Rule,
                    PutPostDeleteAnalyzer.Rule,
                    OneFileOneCore.Rule,
                    NestedEnumAndClass.Rule,
                    ToSelectOptimization.Rule);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Entity—onstructorAnalyzer.Analyze, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxTreeAction(LineLengthAnalyzer.AnalyzeTree);
            context.RegisterSyntaxNodeAction(FluentAnalyzer.Analyze, SyntaxKind.SimpleMemberAccessExpression);
            context.RegisterSyntaxNodeAction(PropertyModifiersAnalyzer.Analyze, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(MethodParamsAnalyzer.Analyze, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(OneFileOneClass.Analyze, SyntaxKind.NamespaceDeclaration);
            context.RegisterSyntaxNodeAction(BooleanPropsNameAnalyzer.Analyze, SyntaxKind.PropertyDeclaration);
            context.RegisterSyntaxNodeAction(SimpleInterfaceAnalizer.Analyze, SyntaxKind.InterfaceDeclaration);
            context.RegisterSyntaxNodeAction(EnumCheck.Analyze, SyntaxKind.EnumDeclaration);
            context.RegisterSyntaxNodeAction(MethodBodyAnalyze.Analyze, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(LanguageAnalyzer.AnalyzeXML, SyntaxKind.SingleLineDocumentationCommentTrivia);
            context.RegisterSyntaxTreeAction(LanguageAnalyzer.Analyze);
            context.RegisterSyntaxNodeAction(SelectAnalyzer.Analyze, SyntaxKind.SimpleMemberAccessExpression);
            context.RegisterSyntaxNodeAction(PutPostDeleteAnalyzer.Analyze, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(OneFileOneCore.Analyze, SyntaxKind.NamespaceDeclaration);
            context.RegisterSyntaxNodeAction(NestedEnumAndClass.Analyze, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(ToSelectOptimization.Analyze, SyntaxKind.InvocationExpression);
        }
    }
}