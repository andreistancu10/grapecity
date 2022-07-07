namespace DigitNow.Domain.DocumentManagement.Business.Common.Export.Pdf.Poco
{
    public class PdfToken
    {
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
        public PdfToken(string name, string value)
        {
            TokenName = name;
            TokenValue = value;
        }
    }
}
