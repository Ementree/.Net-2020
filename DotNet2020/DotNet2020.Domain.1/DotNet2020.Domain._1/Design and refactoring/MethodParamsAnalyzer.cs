using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// анализатор для проверки количества аргументов метода
    /// </summary>
    class MethodParamsAnalyzer
    {
        public const string DiagnosticId = "MethodParamsId";
        public const string CodeFixTitle = "Use Parameter Object or collection instead of paramer list";
        const string Title = "Replace parameter list";
        const string MessageFormat = "Use Parameter Object or collection instead of paramer list";
        const string Category = "Design and refactoring";
        const string Description = "Use Parameter Object or collection instead of paramer list";
        const int ParamsCount = 1;
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            // cразу принимать на вход parameterlistsyntax?
            var paramsList = (context.Node as MethodDeclarationSyntax).ParameterList;
            if (paramsList.Parameters.Count > ParamsCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule,
                paramsList.GetLocation(), paramsList.GetText()));
            }
        }

        //TODO: доделать CodeFix
        public static async Task<Solution> CodeFix(Document document, CodeFixContext context, CancellationToken cancellationToken)
        {
            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var paramList = root.FindNode(diagnosticSpan) as ParameterListSyntax;
            var parameters = paramList.Parameters;
            var types = parameters.Select(p => p.Type);
            var typeSyntax = types.FirstOrDefault();
            var classDeclaration = paramList.Parent.Parent;
            var methodName = (paramList.Parent as MethodDeclarationSyntax).Identifier.ValueText;

            SyntaxNode newRoot = null;
            //if (types.All(t => t == typeSyntax))
            //    newRoot = UseCollection(typeSyntax, paramList, root);
            //else
                newRoot = UseParameterObject(root, classDeclaration, paramList, parameters, methodName);
            return document.WithSyntaxRoot(newRoot).Project.Solution;
        }

        /// <summary>
        /// заменяет параметры метода на parameter object
        /// </summary>
        /// <returns>новое синтаксическое дерево</returns>
        private static SyntaxNode UseParameterObject(SyntaxNode root,
            SyntaxNode classDeclaration, ParameterListSyntax oldList,
            SeparatedSyntaxList<ParameterSyntax> parameters, string methodName)
        {
            var fields = parameters.Select(p =>
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(p.Type)
                    .AddVariables(SyntaxFactory.VariableDeclarator(p.Identifier.ValueText))));

            var paramObj = SyntaxFactory.ClassDeclaration(methodName + "ParameterObject")
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(fields));

            var newList = SyntaxFactory.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().AddRange
                (new ParameterSyntax[] {
                        SyntaxFactory.Parameter(paramObj.Identifier).WithType(SyntaxFactory.ParseTypeName(methodName + "ParameterObject"))
                    }
                ));

            return root.InsertNodesAfter(classDeclaration, new List<SyntaxNode> { paramObj })
                .ReplaceNode(oldList, newList);
        }

        /// <summary>
        /// заменает однотипные параметры метода на коллекцию
        /// </summary>
        /// <returns>новое синтаксическое дерево</returns>
        private static SyntaxNode UseCollection(TypeSyntax type, ParameterListSyntax oldList, SyntaxNode root)
        {
            var newList = SyntaxFactory.ParameterList(new SeparatedSyntaxList<ParameterSyntax>().AddRange
               (new ParameterSyntax[]
               {
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("collection"))
                    .WithType(SyntaxFactory.GenericName(
                        SyntaxFactory.Identifier("IEnumerable"),
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>(new[] {type}))))
               }));
            return root.ReplaceNode(oldList, newList);
        }
    }
}