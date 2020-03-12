using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace DotNet2020
{
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        public CustomViewLocationExpander()
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
