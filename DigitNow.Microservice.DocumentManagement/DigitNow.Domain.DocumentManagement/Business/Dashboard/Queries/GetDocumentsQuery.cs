using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries
{
    public class GetDocumentsQuery : IQuery<GetDocumentsResponse>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DocumentPreprocessFilter PreprocessFilter { get; set; }
        public DocumentPostprocessFilter PostprocessFilter { get; set; }
    }
}