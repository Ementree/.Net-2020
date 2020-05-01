using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DotNet2020.Domain._3.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;

namespace DotNet2020.Domain._3.Helpers
{
    public static class PdfHelper
    {
        const int LeftPadding = 5;
        public static MemoryStream GetPdfofWorkers(List<long> ids, List<SpecificWorkerModel> workers)
        {
            var memoryStream = new MemoryStream();

            var document = new Document(PageSize.A4, 75, 65, 75, 75);
            document.AddTitle("Выгрузка работников");
            document.AddCreationDate();
            PdfWriter writer =  PdfWriter.GetInstance(document, memoryStream);
           
            System.Text.EncodingProvider encProvider = System.Text.CodePagesEncodingProvider.Instance;
            
            Encoding.RegisterProvider(encProvider);
            string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            
            BaseFont baseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            
            Font body = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);
            Font head=new Font(baseFont, 16, Font.NORMAL, BaseColor.BLACK);


            document.Open();

            workers = workers.Where(x => ids.Contains(x.Id)).ToList();

            document.Add(new Paragraph($"Работники", head));
            document.Add(new Paragraph($" ", body));
            
            PdfPTable table=new PdfPTable(5);
            table.TotalWidth = document.PageSize.Width - 72f - 65f;
            table.LockedWidth = true;
            float[] widths = new float[] {2.7f, 4f, 4f, 1.5f, 2.5f };
            table.SetWidths(widths);
            table.HorizontalAlignment = 0;

            PdfPCell tablecell10 = new PdfPCell(new Phrase($"ФИО", body));
            table.AddCell(tablecell10);
            PdfPCell tablecell11 = new PdfPCell(new Phrase($"Должность", body));
            table.AddCell(tablecell11);
            PdfPCell tablecell12 = new PdfPCell(new Phrase($"Компетенции", body));
            table.AddCell(tablecell12);
            PdfPCell tablecell13 = new PdfPCell(new Phrase("Опыт работы", body));
            table.AddCell(tablecell13);
            PdfPCell tablecell14 = new PdfPCell(new Phrase("Предыдущие места работы", body));
            table.AddCell(tablecell14);
            
            foreach (var worker in workers)
            {
                PdfPCell tablecellx0 = new PdfPCell(new Phrase($"{worker.Initials}", body));
                tablecellx0.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx0);

                PdfPCell tablecellx1 = new PdfPCell(new Phrase($"{worker.Position.Name}", body));
                tablecellx1.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx1);

                StringBuilder builder = new StringBuilder("");

                foreach (var competence in worker.SpecificWorkerCompetencesModels)
                {
                    builder.Append(competence.Competence.Competence+", ");
                }

                if (builder.ToString() == "")
                    builder.Append("Не обладает компетенциями");
                else
                {
                    builder.Remove(builder.Length - 2, 2);
                }
                
                PdfPCell tablecellx2 = new PdfPCell(new Phrase($"{builder.ToString()}", body));
                tablecellx2.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx2);
                
                PdfPCell tablecellx3 = new PdfPCell();
                tablecellx3 = new PdfPCell(new Phrase($"{worker.Experience}", body));
                tablecellx3.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx3);
                
                PdfPCell tablecellx4 = new PdfPCell(new Phrase($"{worker.PreviousWorkPlaces}", body));
                tablecellx4.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx4);
            }
            document.Add(table);
            
            document.Close();

            byte[] file = memoryStream.ToArray();
            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);
            ms.Position = 0;

            return ms;
        }
        
        public static MemoryStream GetPdfOfAttestation(long id, DbContext context)
        {
            var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4, 75, 65, 75, 75);
            document.AddTitle("Результаты аттестации");
            document.AddCreationDate();
            PdfWriter writer =  PdfWriter.GetInstance(document, memoryStream);
           
            System.Text.EncodingProvider encProvider = System.Text.CodePagesEncodingProvider.Instance;
            
            Encoding.RegisterProvider(encProvider);
            string fg = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            
            BaseFont baseFont = BaseFont.CreateFont(fg, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            
            Font body = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);
            Font boldBody=new Font(baseFont, 10, Font.BOLD, BaseColor.BLACK);
            Font head=new Font(baseFont, 16, Font.NORMAL, BaseColor.BLACK);

            var attestation = context.Set<AttestationModel>().First(x => x.Id == id);

            context.Entry(attestation).Collection(x => x.AttestationAnswer).Load();

            foreach (var attestationAnswer in attestation.AttestationAnswer)
            {
                attestationAnswer.Answer = context.Set<AnswerModel>().Find(attestationAnswer.AnswerId);
            }

            attestation.AttestationAnswer = attestation.AttestationAnswer.Where(x => x.Answer.IsSkipped == false).OrderBy(x=>x.Answer.NumberOfAsk).ToList();

            var testedCompetences = attestation.IdsTestedCompetences;

            List<CompetencesModel> competencesModels=new List<CompetencesModel>();
            
            if(testedCompetences==null)
                testedCompetences=new List<long>();

            foreach (var testedCompetence in testedCompetences)
            {
                competencesModels.Add(context.Set<CompetencesModel>().Find(testedCompetence));
            }
            
            var worker = context.Set<SpecificWorkerModel>().Find((int)attestation.WorkerId);
            
            document.Open();
            if (worker == null)
            {
                worker=new SpecificWorkerModel();
                document.Add(new Paragraph($"Аттестация удалённого работника", head));
                document.Add(new Paragraph(attestation.Date.ToString("d"), head));
                document.Add(new Paragraph(" ", body));
            }
                
            else
            {
                document.Add(new Paragraph($"{worker.FullName} - результаты аттестации", head));
                document.Add(new Paragraph(attestation.Date.ToString("d"), head));
                document.Add(new Paragraph(" ", body));
            }
            document.Add(new Paragraph("Техническое интервью", head));
            document.Add(new Paragraph(" ", body));
            document.Add(new Paragraph("Блоки компетенций:", boldBody));
            foreach (var competencesModel in competencesModels)
            {
                if (competencesModel == null)
                {
                    document.Add(new Paragraph($"  -   компетенция была удалена", body));
                }
                else
                {
                    document.Add(new Paragraph($"  -   {competencesModel.Competence}", body));
                }
            }
            
            document.Add(new Paragraph(" ", body));
            
            document.Add(new Paragraph("Выявлены пробелы в знаниях:", boldBody));
            document.Add(new Paragraph($"{attestation.Problems}", body));
            
            document.Add(new Paragraph(" ", body));
            
            document.Add(new Paragraph("Анализ результатов работы на проекте", head));
            document.Add(new Paragraph("Данные из другого проекта", body));
            
            document.Add(new Paragraph(" ", body));
            
            document.Add(new Paragraph("Дальнейшие действия", head));
            document.Add(new Paragraph($"{attestation.NextMoves}", body));
            
            document.Add(new Paragraph(" ", body));
            
            document.Add(new Paragraph("Обратная связь от руководителя проекта:", boldBody));
            document.Add(new Paragraph($"{attestation.Feedback}", body));
            
            document.Add(new Paragraph(" ", body));

            PdfPTable table=new PdfPTable(4);
            table.TotalWidth = document.PageSize.Width - 72f - 65f;
            table.LockedWidth = true;
            float[] widths1 = new float[] { 0.55f, 4f, 0.7f, 4f };
            table.SetWidths(widths1);
            table.HorizontalAlignment = 0;
            
            PdfPCell tablecell11 = new PdfPCell(new Phrase($"№", body));
            table.AddCell(tablecell11);

            PdfPCell tablecell12 = new PdfPCell(new Phrase($"Вопрос", body));
            table.AddCell(tablecell12);

            PdfPCell tablecell13 = new PdfPCell(new Phrase("Верно", body));
            table.AddCell(tablecell13);

            PdfPCell tablecell14 = new PdfPCell(new Phrase("Комментарий", body));
            table.AddCell(tablecell14);
            
            foreach (var answer in attestation.AttestationAnswer)
            {
                PdfPCell tablecellx1 = new PdfPCell(new Phrase($"{answer.Answer.NumberOfAsk}", body));
                tablecellx1.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx1);
                
                PdfPCell tablecellx2 = new PdfPCell(new Phrase($"{answer.Answer.Question}", body));
                tablecellx2.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx2);
                
                PdfPCell tablecellx3 = new PdfPCell();
                if (answer.Answer.IsRight)
                    tablecellx3 = new PdfPCell(new Phrase("+", body));
                else
                    tablecellx3 = new PdfPCell(new Phrase("-", body));
                tablecellx3.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx3);
                
                PdfPCell tablecellx4 = new PdfPCell(new Phrase($"{answer.Answer.Commentary}", body));
                tablecellx4.PaddingLeft = LeftPadding;
                table.AddCell(tablecellx4);
            }
            document.Add(table);
            document.Close();

            byte[] file = memoryStream.ToArray();
            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);
            ms.Position = 0;

            return ms;
        }
    }
}