using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNet2020.Domain._1
{
    internal class CommentContext
    {
        private readonly SyntaxNodeAnalysisContext nodeContext;
        private readonly SyntaxTreeAnalysisContext treeContext;
        private readonly bool IsNode;

        internal CommentContext(SyntaxNodeAnalysisContext сontext)
        {
            nodeContext = сontext;
            IsNode = true;
        }

        internal CommentContext(SyntaxTreeAnalysisContext сontext)
        {
            treeContext = сontext;
            IsNode = false;
        }

        internal void ReportDiagnostic(Diagnostic diagnostic)
        {
            if (IsNode) nodeContext.ReportDiagnostic(diagnostic);
            else treeContext.ReportDiagnostic(diagnostic);
        }
    }
}
