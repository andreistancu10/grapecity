using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DocumentTypeViewModel
    {
        public DocumentType Id { get; set; }
        public string Name => Id.ToString();
        public string Label { get; set; }
    }
}
