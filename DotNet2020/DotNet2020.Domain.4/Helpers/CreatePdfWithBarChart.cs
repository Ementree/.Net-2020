
using Kendo.Mvc.Examples.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Text;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using FontFamily = Telerik.Documents.Core.Fonts.FontFamily;
using FontStyles = Telerik.Documents.Core.Fonts.FontStyles;
using FontWeights = Telerik.Documents.Core.Fonts.FontWeights;
using Size = Telerik.Documents.Primitives.Size;
using Point = Telerik.Documents.Primitives.Point;
using Rect = Telerik.Documents.Primitives.Rect;

using Editing = Telerik.Windows.Documents.Fixed.Model.Editing;

using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Editing.Tables;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Documents.Media;
using System.Drawing;
using System.Diagnostics;

namespace Kendo.Mvc.Examples


{
    public class CreatePdfWithBarChart
    {
        private static readonly double chartWidth = 600;
        private static readonly double chartHeight = 360;
        private static readonly double markerAreaWidth = 60;
        private static readonly double marginTop = 200;
        private static readonly double valuesMargin = 10;
        private static readonly double rectSize = 8;
        private static readonly double barMargin = 2;
        private static readonly int markersCount = 7;
        private static readonly RgbColor[] colors =
            {
                new RgbColor(46, 204, 113),
                new RgbColor(155, 89, 182),
                new RgbColor(52, 152, 219),
                new RgbColor(241, 196, 15),
                new RgbColor(230, 126, 34),
                new RgbColor(231, 76, 60)
            };

        private static ProductPdfProcessing[] products;
        private static Dictionary<int, bool> quartersToExport;

        private static bool q1 = true;
        private static bool q2 = true;
        private static bool q3 = true;
        private static bool q4 = true;

        private static int ExportedProductsCount = 1;
        private static double StepValue = 5000;

        public static void ExampleViewModel()
        {
            products = ProductPdfProcessing.GetProducts();
            quartersToExport = new Dictionary<int, bool>();

            InitializeData();
        }

        private static Size MeasureText(FixedContentEditor editor, string text)
        {
            Block block = CreateBlock(editor);
            block.InsertText(text);

            return block.Measure();
        }

        private static void DrawText(FixedContentEditor editor, string text, double width = double.PositiveInfinity, HorizontalAlignment alignment = HorizontalAlignment.Left)
        {
            Block block = CreateBlock(editor);
            block.HorizontalAlignment = alignment;
            block.InsertText(text);
            editor.DrawBlock(block, new Size(width, double.PositiveInfinity));
        }

        private static Block CreateBlock(FixedContentEditor editor)
        {
            Block block = new Block();
            block.TextProperties.CopyFrom(editor.TextProperties);
            block.GraphicProperties.CopyFrom(editor.GraphicProperties);

            return block;
        }

        private static void InitializeData()
        {
            quartersToExport.Add(0, q1);
            quartersToExport.Add(1, q2);
            quartersToExport.Add(2, q3);
            quartersToExport.Add(3, q4);
            Products = new List<int>();
            for (int currentIndex = 0; currentIndex < products.Length; currentIndex++)
            {
                Products.Add(currentIndex + 1);
            }
        }

        public ICommand Save { get; private set; }

        public static List<int> Products { get; private set; }


        public static RadFixedDocument CreateDocument(bool cb1, bool cb2, bool cb3, bool cb4, string products, double currency)
        {
            RadFixedDocument document = new RadFixedDocument();
            RadFixedPage page = document.Pages.AddPage();
            page.Size = new Size(792, 1128);
            q1 = cb1;
            q2 = cb2;
            q3 = cb3;
            q4 = cb4;
            ExportedProductsCount = int.Parse(products);
            StepValue = currency;
            FixedContentEditor editor = new FixedContentEditor(page);
            ExampleViewModel();
            DrawBarChartContent(editor);
            DrawTableReportContent(editor);

            return document;
        }

        private static void DrawTableReportContent(FixedContentEditor editor)
        {
            RgbColor headerColor = new RgbColor(79, 129, 189);
            RgbColor bordersColor = new RgbColor(149, 179, 215);
            RgbColor alternatingRowColor = new RgbColor(219, 229, 241);

            Border border = new Border(1, Editing.BorderStyle.Single, bordersColor);

            Table table = new Table();
            table.Borders = new TableBorders(border);
            table.LayoutType = TableLayoutType.FixedWidth;
            table.DefaultCellProperties.Borders = new TableCellBorders(border, border, border, border);
            table.DefaultCellProperties.Padding = new Telerik.Documents.Primitives.Thickness(2);

            TableRow headerRow = table.Rows.AddTableRow();
            TableCell headerCell = headerRow.Cells.AddTableCell();
            headerCell.Borders = new TableCellBorders(new Border(BorderStyle.None));
            headerCell.ColumnSpan = GetQuartersToExportCount() + 1;
            Block headerBlock = headerCell.Blocks.AddBlock();
            headerBlock.HorizontalAlignment = HorizontalAlignment.Center;


            TableRow quartersRow = table.Rows.AddTableRow();
            TableCell cell = quartersRow.Cells.AddTableCell();
            cell.Background = headerColor;
            cell.Borders = new TableCellBorders(border, border, border, border, null, border);

            foreach (KeyValuePair<int, bool> quarter in quartersToExport)
            {
                if (quarter.Value)
                {
                    TableCell quarterCell = quartersRow.Cells.AddTableCell();
                    quarterCell.Background = headerColor;

                    Block quarterBlock = quarterCell.Blocks.AddBlock();
                    quarterBlock.GraphicProperties.FillColor = RgbColors.White;
                    quarterBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    quarterBlock.InsertText(String.Format("Q{0}", quarter.Key + 1));
                }
            }

            for (int i = 0; i < ExportedProductsCount; i++)
            {
                RgbColor rowColor = i % 2 == 0 ? alternatingRowColor : RgbColors.White;
                ProductPdfProcessing product = products[i];

                TableRow productRow = table.Rows.AddTableRow();
                TableCell productNameCell = productRow.Cells.AddTableCell();
                productNameCell.Background = rowColor;
                Block nameBlock = productNameCell.Blocks.AddBlock();
                nameBlock.InsertText(product.Name);

                foreach (KeyValuePair<int, bool> quarter in quartersToExport)
                {
                    if (quarter.Value)
                    {
                        TableCell quarterAmountCell = productRow.Cells.AddTableCell();
                        quarterAmountCell.Background = rowColor;
                        Block amountBlock = quarterAmountCell.Blocks.AddBlock();
                        amountBlock.HorizontalAlignment = HorizontalAlignment.Right;
                        amountBlock.InsertText(string.Format("{0:C}", product.Q[quarter.Key]));
                    }
                }
            }

            double top = marginTop + chartHeight + 50;
            double left = GetLeftMargin(editor.Root.Size.Width);
            table.Draw(editor, new Rect(left, top, chartWidth, double.PositiveInfinity));
        }

        private static double GetLeftMargin(double pageWidth)
        {
            return (pageWidth - chartWidth) / 2;
        }

        private static void DrawBarChartContent(FixedContentEditor editor)
        {
            editor.GraphicProperties.IsFilled = false;

            double leftMargin = GetLeftMargin(editor.Root.Size.Width);
            double offsetX;
            double offsetY;

            DrawChartFrame(leftMargin, editor, out offsetX, out offsetY);

            double offset = 20;
            double textWidth = 0;
            double rectMargin = 2;

            for (int i = 0; i < ExportedProductsCount; i++)
            {
                textWidth += rectSize + rectMargin + offset;
                textWidth += MeasureText(editor, products[i].Name).Width;
            }

            offsetX = leftMargin + ((chartWidth - textWidth) / 2);
            offsetY += 20;
            for (int i = 0; i < ExportedProductsCount; i++)
            {
                editor.Position.Translate(offsetX, offsetY);
                Tiling tiling = CreateTiling(offsetX, offsetY, rectSize, i);

                Block block = new Block();
                block.GraphicProperties.FillColor = tiling;
                block.GraphicProperties.IsStroked = false;
                block.InsertRectangle(new Rect(0, 0, rectSize, rectSize));
                block.GraphicProperties.FillColor = RgbColors.Black;
                block.InsertText(" " + products[i].Name);
                editor.DrawBlock(block);
                offsetX += block.DesiredSize.Width + offset;
            }

            offsetX = leftMargin;

            offsetY += 30;
            double markerHeight = (chartHeight - (offsetY - marginTop)) / markersCount;
            editor.Position.Translate(offsetX, offsetY);

            for (int i = markersCount - 1; i >= 0; i--)
            {
                DrawText(editor, string.Format("{0:C}", i * StepValue), markerAreaWidth, HorizontalAlignment.Right);
                if (i > 0)
                {
                    offsetY += markerHeight;
                    editor.Position.Translate(offsetX, offsetY);
                }
            }

            offsetX = leftMargin + markerAreaWidth + valuesMargin;
            double center = MeasureText(editor, "X").Height / 2;
            offsetY += center;
            double valueHeight = markerHeight / StepValue;
            double dataAreaWidth = chartWidth - markerAreaWidth - 2 * valuesMargin;

            double sectionWidth = dataAreaWidth / GetQuartersToExportCount();
            double barWidth = (sectionWidth - 2 * valuesMargin - 2 * ExportedProductsCount * barMargin) / ExportedProductsCount;
            for (int j = 0; j < quartersToExport.Keys.Count; j++)
            {
                if (!quartersToExport[j])
                {
                    continue;
                }

                editor.Position.Translate(offsetX, offsetY + 5);
                editor.GraphicProperties.FillColor = RgbColors.Black;
                DrawText(editor, string.Format("Q{0}", j + 1), sectionWidth, HorizontalAlignment.Center);
                editor.Position.Translate(0, 0);
                offsetX += valuesMargin;
                for (int i = 0; i < ExportedProductsCount; i++)
                {
                    ProductPdfProcessing product = products[i];
                    double h = product.Q[j] * valueHeight;
                    offsetX += barMargin;
                    Tiling tiling = CreateTiling(offsetX, offsetY - h, barWidth, i);
                    editor.GraphicProperties.FillColor = tiling;
                    editor.DrawRectangle(new Rect(offsetX, offsetY - h, barWidth, h));
                    offsetX += barWidth + barMargin;
                }

                offsetX += valuesMargin;
            }

            offsetX = leftMargin + markerAreaWidth + valuesMargin;
            DrawBarLine(editor, offsetX, offsetY, dataAreaWidth);
        }

        private static void DrawChartFrame(double leftMargin, FixedContentEditor editor, out double offsetX, out double offsetY)
        {
            offsetX = leftMargin;
            offsetY = marginTop;
            editor.DrawRectangle(new Rect(offsetX, offsetY, chartWidth, chartHeight));
            offsetY += 10;
            editor.Position.Translate(offsetX, offsetY);

            editor.TextProperties.FontSize = 18;
            editor.TextProperties.TrySetFont(new FontFamily("Calibri"), FontStyles.Normal, FontWeights.Bold);
            DrawText(editor, "2017", chartWidth, HorizontalAlignment.Center);

            offsetY += 30;
            editor.Position.Translate(offsetX, offsetY);

            editor.TextProperties.TrySetFont(new FontFamily("Calibri"));
            editor.TextProperties.FontSize = 10;
            editor.GraphicProperties.IsFilled = true;
            editor.GraphicProperties.IsStroked = false;
        }

        private static Tiling CreateTiling(double offsetX, double offsetY, double width, int i)
        {

            Tiling tiling = new Tiling(new Rect(0, 0, width, 2));
            tiling.Position.Translate(offsetX, offsetY);
            var tilingEditor = new FixedContentEditor(tiling);
            tilingEditor.GraphicProperties.IsStroked = false;
            tilingEditor.GraphicProperties.FillColor = colors[i];
            tilingEditor.DrawRectangle(new Rect(0, 0, width, 1));
            LinearGradient gradient = new LinearGradient(new Point(0, 0), new Point(width, 0));
            gradient.GradientStops.Add(new Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop(new RgbColor(255, 146, 208, 80), 0));
            gradient.GradientStops.Add(new Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop(RgbColors.White, .5));
            gradient.GradientStops.Add(new Telerik.Windows.Documents.Fixed.Model.ColorSpaces.GradientStop(new RgbColor(255, 146, 208, 80), 1));
            tilingEditor.GraphicProperties.FillColor = gradient;
            tilingEditor.DrawRectangle(new Rect(0, 1, width, 1));

            return tiling;
        }

        private static void DrawBarLine(FixedContentEditor editor, double offsetX, double offsetY, double width)
        {
            editor.GraphicProperties.FillColor = RgbColors.Black;
            editor.GraphicProperties.StrokeThickness = 1;
            editor.GraphicProperties.IsFilled = true;
            editor.GraphicProperties.IsStroked = true;
            editor.DrawLine(new Point(offsetX, offsetY), new Point(offsetX + width, offsetY));
        }

        private static int GetQuartersToExportCount()
        {
            int counter = 0;
            foreach (bool shouldExportQ in quartersToExport.Values)
            {
                if (shouldExportQ)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}
