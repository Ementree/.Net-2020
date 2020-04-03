using System;
using System.Collections.Generic;
using System.Linq;

namespace Kendo.Mvc.Examples.Models
{
    public class NavigationItem
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public IDictionary<string, string> Title { get; set; }
        public IDictionary<string, string> Meta { get; set; }
        public IDictionary<string, string> Description { get; set; }
        public IDictionary<string, IEnumerable<ExampleFile>> Sources { get; set; }
        public string[] Packages { get; set; }

        public bool ShouldInclude(string package = "aspnet-core")
        {
            if (Packages == null || Packages.Length == 0)
            {
                return true;
            }

            var invert = false;
            var match = false;

            foreach (var packageName in Packages)
            {
                var name = packageName;
                if (name[0] == '!')
                {
                    invert = true;
                    name = name.Substring(1);
                }

                if (name == package)
                {
                    match = true;
                }
            }

            return (!invert && match) || (invert && !match);
        }
    }
}
