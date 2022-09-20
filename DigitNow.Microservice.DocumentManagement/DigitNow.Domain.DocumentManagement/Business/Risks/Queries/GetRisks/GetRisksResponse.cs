using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRisks
{
    internal class GetRisksResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<RiskViewModel> Items { get; set; }
    }
}
