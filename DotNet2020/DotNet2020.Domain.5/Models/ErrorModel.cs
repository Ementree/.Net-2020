using System;

namespace DotNet2020.Domain._5.Models
{
    public class ErrorModel
    {
        public ErrorModel(string title, string message)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
