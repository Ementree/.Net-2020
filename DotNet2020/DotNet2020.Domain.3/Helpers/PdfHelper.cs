using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNet2020.Domain._3.Models;
using DotNet2020.Domain._3.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DotNet2020.Domain._3.Helpers
{
    public static class PdfHelper
    {
        public static void GetPDFofWorkers(List<long> ids, SpecificWorkerRepository _workers)
        {
            var document = new Document();
            using (var writer = PdfWriter.GetInstance(document, new FileStream("Files/workertest.pdf", FileMode.Create)))
            {
                document.Open();

                var helvetica = new Font(Font.FontFamily.HELVETICA, 12);
                var helveticaBase = helvetica.GetCalculatedBaseFont(false);

                var workers = WorkerOutputModelHelper.GetList(_workers);


                workers = workers.Where(x => ids.Contains(x.Worker.Id)).ToList();

                foreach (var worker in workers)
                {
                    writer.DirectContent.BeginText();
                    writer.DirectContent.SetFontAndSize(helveticaBase, 12f);
                    writer.DirectContent.ShowTextAligned(Element.ALIGN_LEFT, worker.Worker.Id + "-" + worker.Worker.Name + "-" + worker.Worker.Position + "-"+ worker.Worker.Salary, 35, 766, 0);
                    writer.DirectContent.EndText();
                    document.NewPage();
                }

                document.Close();
                writer.Close();
            }
        }
    }
}