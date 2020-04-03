using System;
using System.Collections.Generic;
using System.Linq;

namespace Kendo.Mvc.Examples.Models
{
    public class NavigationWidget : NavigationItem
    {
        public string Api { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Category { get; set; }
        public string SpriteCssClass { get; set; }
        public bool Expanded { get; set; }
        public bool New { get; set; }
        public bool Updated { get; set; }
        public bool Beta { get; set; }
        public IDictionary<string, string> Documentation { get; set; }
        public IDictionary<string, string> Forum { get; set; }
        public List<NavigationExample> Items { get; set; }
    }
}
