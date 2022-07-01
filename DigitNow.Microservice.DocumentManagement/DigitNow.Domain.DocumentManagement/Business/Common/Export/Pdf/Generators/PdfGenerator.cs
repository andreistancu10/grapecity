using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators
{
    public interface IPdfGenerator
    {
        IPdfGenerator SetToken(PdfToken token);
        Task<FileContent> GenerateAsync(string filePath, string pdfName);
    }
    public class PdfGenerator : IPdfGenerator
    {
        public List<PdfToken> _tokens = new List<PdfToken>();
        public async Task<FileContent> GenerateAsync(string filePath, string pdfName)
        {
            var html = await File.ReadAllTextAsync(filePath);
            foreach (var item in _tokens)
            {
                html = html.Replace(item.TokenName, item.TokenValue);
            }

            var pdf = OpenHtmlToPdf.Pdf
                .From(html)
                .Content();

            return new FileContent(pdfName, "application/pdf", pdf);
        }

        public IPdfGenerator SetToken(PdfToken token)
        {
            _tokens.Add(token);
            return this;
        }
    }
}
