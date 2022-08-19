using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Internal;
using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
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

            var localDistPath = Path.Combine(Directory.GetCurrentDirectory(), "temp");

            var pdfContent = CustomPdfDocument
                .Containing(html)
                .WithGlobalSetting("tempFolder", localDistPath)
                .Content();

            return new FileContent(pdfName, "application/pdf", pdfContent);
        }

        private static string GetTemplateContent(string templateName)
        {
            var templateResourceName = _currentAssemblyResourceNames.FirstOrDefault(x => x.EndsWith(templateName));
            if (templateResourceName == null) throw new ArgumentNullException();

            var stream = _currentAssembly.GetManifestResourceStream(templateResourceName);

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
