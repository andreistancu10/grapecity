using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class DocumentsExportQuery : IQuery<List<ExportDocumentViewModel>>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public DocumentFilter Filter { get; set; }
    }
}