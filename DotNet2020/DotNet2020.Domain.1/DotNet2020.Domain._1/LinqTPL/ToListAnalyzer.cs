using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    /// <summary>
    /// Анализатор для запрета ToList() и запросов к БД в цикле
    /// </summary>
    class ToListAnalyzer
    {
        public const string DiagnosticId = "ToListId";
        public const string CodeFixTitle = "Use .Contains() or subqueries";
        const string Title = "Do not use ToList() or database queries in loop";
        const string Category = "Linq & TPL";
        const string MessageFormat = "Do not use ToList() or database queries in loop";
        const string Description = "Do not use ToList() or database queries in loop";
        public static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var node = context.Node;

            var soutceText = node.GetText();
            var text = soutceText.ToString();
            var keywords = new List<string> { "ToList" };
            var isContain = false;
            foreach (var k in keywords)
            {
                if (text.Contains(k))
                {
                    isContain = true;
                    break;
                }
            }
            if (isContain)
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), node.GetText().ToString()));
        }

        public Task<Solution> CodeFix()
        {
            throw new NotImplementedException();
        }
    }
}