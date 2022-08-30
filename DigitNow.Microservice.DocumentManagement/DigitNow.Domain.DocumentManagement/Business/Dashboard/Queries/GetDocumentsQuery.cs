using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries
{
    public class GetDocumentsQuery : IQuery<GetDocumentsResponse>
    {
        public int LanguageId { get; set; } = LanguagesUtils.RomanianLanguageId;
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DocumentFilter Filter { get; set; }
    }
}