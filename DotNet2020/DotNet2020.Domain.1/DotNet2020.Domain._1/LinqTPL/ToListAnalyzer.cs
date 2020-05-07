using Microsoft.CodeAnalysis;
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

        public void Analyze()
        {
            throw new NotImplementedException();
        }

        public Task<Solution> CodeFix()
        {
            throw new NotImplementedException();
        }

        IQueryable<int> collection;
        void Example()
        {
            foreach(var item in collection)
            {
                var smth = collection.Where(x => x > 0).ToList();
            } 
        } 
    }
}