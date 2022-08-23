using DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<PdfGenerator> _logger;

        public PdfGenerator(IWebHostEnvironment hostingEnvironment, ILogger<PdfGenerator> logger)
        {
           _webHostEnvironment = hostingEnvironment;
            _logger = logger;
        }
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

            try
            {
                return ExportByBlink(html, pdfName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            try
            {
                return ExportByWebKit(html, pdfName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        private FileContent ExportByBlink(string html, string pdfName)
        {
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
            BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "BlinkBinariesWindows");
            blinkConverterSettings.BlinkPath = path;

            htmlConverter.ConverterSettings = blinkConverterSettings;

            var pdfDocument = htmlConverter.Convert(html, string.Empty);

            using (MemoryStream stream = new MemoryStream())
            {
                pdfDocument.Save(stream);
                pdfDocument.Close(true);
                var pdfBytes = stream.ToArray();
                return new FileContent(pdfName, "application/pdf", pdfBytes);

            }
        }

        private FileContent ExportByWebKit(string html, string pdfName)
        {
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            
            var pdfDocument = htmlConverter.Convert(html, string.Empty);

            using (MemoryStream stream = new MemoryStream())
            {
                pdfDocument.Save(stream);
                pdfDocument.Close(true);
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
