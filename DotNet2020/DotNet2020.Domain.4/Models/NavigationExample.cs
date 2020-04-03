using System;
using System.Linq;

namespace Kendo.Mvc.Examples.Models
{
    public class NavigationExample : NavigationItem
    {
        public string Url { get; set; }
        public bool New { get; set; }
        public bool Updated { get; set; }
        public string Group { get; set; }
        public string Api { get; set; }
    }
}
