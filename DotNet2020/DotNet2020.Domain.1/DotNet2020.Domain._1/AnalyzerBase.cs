using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNet2020.Domain._1
{
    public abstract class AnalyzerBase : DiagnosticAnalyzer
    {
        public abstract AnalyzerInfo CreateAnalyzerInfo();

        protected readonly AnalyzerInfo Info;

        public AnalyzerBase()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Info = CreateAnalyzerInfo();
        }
    }
}
