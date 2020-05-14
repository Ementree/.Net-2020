using System;

namespace DotNet2020.Domain._1
{
    public class AnalyzerInfo
    {
        public AnalyzerInfo(string codeFixTitle, string title, string messageFormat, AnalyzerCategories category,
            string description)
        {
            CodeFixTitle = codeFixTitle ?? throw new ArgumentNullException(nameof(codeFixTitle));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            MessageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
            Category = category;
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public string CodeFixTitle { get; private set; }

        public string Title { get; private set; }

        public string MessageFormat { get; private set; }

        public AnalyzerCategories Category { get; private set; }

        public string Description { get; private set; }
    }
}