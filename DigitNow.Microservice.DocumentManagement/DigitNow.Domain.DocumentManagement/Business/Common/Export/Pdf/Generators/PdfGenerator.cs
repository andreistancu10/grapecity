using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Reflection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Generators
{
    public interface IPdfGenerator
    {
        IPdfGenerator SetTemplateName(string templateName);
        IPdfGenerator SetToken(PdfToken token);
        FileContent Generate(string pdfName);
    }

    public class PdfGenerator : IPdfGenerator
    {
        private static Assembly _currentAssembly = typeof(PdfGenerator).Assembly;
        private static string[] _currentAssemblyResourceNames = _currentAssembly.GetManifestResourceNames();
        
        private readonly List<PdfToken> _tokens = new List<PdfToken>();
        private string _templateName;

        public IPdfGenerator SetTemplateName(string templateName)
        {
            _templateName = templateName;
            return this;
        }

        public IPdfGenerator SetToken(PdfToken token)
        {
            _tokens.Add(token);
            return this;
        }

        public FileContent Generate(string pdfName)
        {
            var html = GetTemplateContent(_templateName);
            foreach (var item in _tokens)
            {
                html = html.Replace(item.TokenName, item.TokenValue);
            }

            return CreatePdf(html, pdfName);
        }

        private FileContent CreatePdf(string html, string pdfName)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            graphics.DrawString(html, font, PdfBrushes.Black, new PointF(0, 0));
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);
                document.Close(true);
                var pdfBytes = stream.ToArray();
                return new FileContent(pdfName, "application/pdf", pdfBytes);
            }
        }


        private static string GetTemplateContent(string templateName)
        {
            var templateResourceName = _currentAssemblyResourceNames.FirstOrDefault(x => x.EndsWith(templateName));
            if (templateResourceName == null) throw new ArgumentException("Template resource name cannot be null", nameof(templateName));

            var stream = _currentAssembly.GetManifestResourceStream(templateResourceName);

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
