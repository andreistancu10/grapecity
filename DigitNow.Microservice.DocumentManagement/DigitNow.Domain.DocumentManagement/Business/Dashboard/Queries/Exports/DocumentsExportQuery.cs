using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class DocumentsExportQuery : IQuery<List<ExportDocumentViewModel>>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DocumentFilter Filter { get; set; }
    }
}