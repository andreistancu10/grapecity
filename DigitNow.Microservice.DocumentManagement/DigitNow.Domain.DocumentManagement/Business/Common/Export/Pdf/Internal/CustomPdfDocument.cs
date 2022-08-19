using OpenHtmlToPdf;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Internal
{
    internal sealed class CustomPdfDocument : IPdfDocument
    {
        private readonly string _html;
        private readonly IDictionary<string, string> _globalSettings;
        private readonly IDictionary<string, string> _objectSettings;

        private CustomPdfDocument(string html, IDictionary<string, string> globalSettings, IDictionary<string, string> objectSettings)
        {
            _html = html;
            _globalSettings = globalSettings;
            _objectSettings = objectSettings;
        }

        public static CustomPdfDocument Containing(string html)
        {
            return new CustomPdfDocument(
                html,
                new Dictionary<string, string>(),
                new Dictionary<string, string>());
        }

        public IPdfDocument WithGlobalSetting(string key, string value)
        {
            var globalSettings = _globalSettings.ToDictionary(e => e.Key, e => e.Value);

            globalSettings[key] = value;

            return new CustomPdfDocument(_html, globalSettings, _objectSettings);
        }

        public IPdfDocument WithObjectSetting(string key, string value)
        {
            var objectSetting = _objectSettings.ToDictionary(e => e.Key, e => e.Value);

            objectSetting[key] = value;

            return new CustomPdfDocument(_html, _globalSettings, objectSetting);
        }

        public byte[] Content()
        {
            var temporaryPath = string.Empty;
            if (_globalSettings.ContainsKey("tempFolder"))
            {
                var distPath = _globalSettings["tempFolder"];
                if (!Directory.Exists(distPath)) Directory.CreateDirectory(distPath);
                
                temporaryPath = Path.Combine(_globalSettings["tempFolder"], TemporaryFilename());

                _globalSettings.Remove("tempFolder");
            }
            else
            {
                temporaryPath = TemporaryPdf.TemporaryFilePath();
            }

            return ReadContentUsingTemporaryFile(temporaryPath);
        }

        private byte[] ReadContentUsingTemporaryFile(string temporaryFilename)
        {
            _globalSettings["out"] = temporaryFilename;

            InvokeConvertToPdf();

            var content = TemporaryPdf.ReadTemporaryFileContent(temporaryFilename);

            TemporaryPdf.DeleteTemporaryFile(temporaryFilename);

            return content;
        }

        private void InvokeConvertToPdf()
        {
            var targetAssembly = typeof(IPdfDocument).Assembly;

            var foundType = targetAssembly.GetTypes().First(x => x.Name.EndsWith("HtmlToPdfConverterProcess"));

            var targetMethod = foundType.GetMethods().First(x => x.Name.ToLowerInvariant().Equals("ConvertToPdf".ToLowerInvariant()));

            targetMethod.Invoke(null, new object[] { _html, _globalSettings, _objectSettings });
        }

        private static string TemporaryFilename()
        {
            return Guid.NewGuid().ToString("N") + ".pdf";
        }
    }
}
