using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicForms
{
    public class GetHistoricalArchiveDocumentsResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<HistoricalArchiveDocumentsViewModel> Items { get; set; }
    }
}
