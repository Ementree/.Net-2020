using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DotNet2020.Domain._4
{
    public class CustomViewLocationExpander2 : IViewLocationExpander
    {
        public CustomViewLocationExpander2()
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            List<string> expandedViewLocations = new List<string>();

            expandedViewLocations.AddRange(viewLocations);
            expandedViewLocations.Add("/Views/{1}/{0}.cshtml");
            return expandedViewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }
    }
}
