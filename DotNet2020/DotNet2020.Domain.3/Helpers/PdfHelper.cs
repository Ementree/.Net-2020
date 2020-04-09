using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                System.Text.EncodingProvider encProvider = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(encProvider);
                string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont fgBaseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fgFont = new Font(fgBaseFont, 10, Font.NORMAL, BaseColor.GREEN);
                
                document.Open();
                
                var workers = WorkerOutputModelHelper.GetList(_workers);


                workers = workers.Where(x => ids.Contains(x.Worker.Id)).ToList();

                foreach (var worker in workers)
                {
                    writer.DirectContent.BeginText();
                    writer.DirectContent.SetFontAndSize(fgBaseFont, 12f);
                    writer.DirectContent.ShowTextAligned(Element.ALIGN_LEFT, worker.Worker.Id + "-" + worker.Worker.Name + "-" + worker.Worker.Position + "-"+ worker.Worker.Salary, 35, 766, 0);
                    writer.DirectContent.EndText();
                    document.NewPage();
                }

                document.Close();
                writer.Close();
            }
        }
        
        public static void GetPdfOfAttestation(long id, AttestationRepository attestationRepository, SpecificWorkerRepository workerRepository)
        {
            var document = new Document();
            using (var writer = PdfWriter.GetInstance(document, new FileStream("Files/attestation.pdf", FileMode.Create)))
            {
                System.Text.EncodingProvider encProvider = System.Text.CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(encProvider);
                string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont fgBaseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fgFont = new Font(fgBaseFont, 10, Font.NORMAL, BaseColor.GREEN);
                
                document.Open();
                
                var attestation = AttestationAnswerOutputModelHelper
                    .GetList(attestationRepository)
                    .First(x=>x.Attestation.Id==id);

                var worker = workerRepository.GetById(attestation.Attestation.WorkerId.Value);

                writer.DirectContent.BeginText();
                writer.DirectContent.SetFontAndSize(fgBaseFont, 12f);
                writer.DirectContent.ShowTextAligned(Element.ALIGN_CENTER, $"Attestation {worker.Name}", 200, 766, 0);

                int x = 1;
                
                foreach (var answer in attestation.Answers)
                {
                    x++;
                    writer.DirectContent.ShowTextAligned(Element.ALIGN_LEFT, 
                        $"Номер вопроса:{answer.NumberOfAsk} \n " +
                             $"Вопрос:{answer.Question}\n Правильный?{answer.IsRight}\n Пропущенный?{answer.IsSkipped}", 1, 765-15*x, 0);
                }
                
                writer.DirectContent.EndText();
                
                document.Close();
                writer.Close();
            }
        }
    }
}