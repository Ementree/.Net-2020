using System;
using System.IO;
using System.Linq;

namespace Kendo.Mvc.Examples.Models
{
    public class ExampleFile
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public bool Exists(string contentRootPath)
        {
            NormalizeUrl(contentRootPath);

            return File.Exists(contentRootPath + Url);
        }

        private void NormalizeUrl(string contentRootPath)
        {
            var normalized = Url.Replace("-", "_");

            if (normalized.StartsWith("~"))
            {
                normalized = normalized.Substring(1);
            }

            if (File.Exists(contentRootPath + normalized))
            {
                Url = normalized;
                Name = Name.Replace("-", "_");
            }
        }
    }
}