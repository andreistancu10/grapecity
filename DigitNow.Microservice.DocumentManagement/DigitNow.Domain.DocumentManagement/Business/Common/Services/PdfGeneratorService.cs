using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business._Common.Documents.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public async Task<FileContent> GeneratePdfAsync(string filePath, string pdfName, List<Tuple<string, string>> replaceableStrings)
        {
            var html = await File.ReadAllTextAsync(filePath);

            replaceableStrings.ForEach(tuple =>
            {
                html = html.Replace(tuple.Item1, tuple.Item2);
            });

            var pdf = OpenHtmlToPdf.Pdf
                .From(html)
                .Content();
            return new FileContent(pdfName, "application/pdf", pdf);
        }
    }
}
