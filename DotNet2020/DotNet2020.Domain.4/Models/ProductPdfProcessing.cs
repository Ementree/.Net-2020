using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kendo.Mvc.Examples.Models
{
    public class ProductPdfProcessing
    {
        public ProductPdfProcessing(string name, params double[] q)
        {
            this.Name = name;
            this.Q = q;
        }

        public string Name { get; set; }
        public double[] Q { get; set; }

        public static ProductPdfProcessing[] GetProducts()
        {
            return new ProductPdfProcessing[] {
                new ProductPdfProcessing("Product1", 25000, 30000, 15000, 25000),
                new ProductPdfProcessing("Product2", 20000, 28000, 12000, 20000),
                new ProductPdfProcessing("Product3", 14000, 28000, 8400, 14000),
                new ProductPdfProcessing("Product4", 7000, 14000, 4200, 7000),
                new ProductPdfProcessing("Product5", 7700, 15400, 4620, 7700),
                new ProductPdfProcessing("Product6", 13090, 26180, 7854, 13090) };
        }
    }
}