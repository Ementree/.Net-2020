using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNet2020.Domain._1
{
    interface IAnalyzer
    {
        string DiagnosticId { get; }
        string CodeFixTitle { get; }
        string Title { get; }
        string MessageFormat { get; }
        string Category { get; }
        string Description { get; }
        DiagnosticDescriptor Rule { get; }

        // нужно ли создавать поле/свойство для типов (syntaxkind), которые будут анализироваться?

        void Analyze();
        Task<Solution> ChangeSolution();
    }
}