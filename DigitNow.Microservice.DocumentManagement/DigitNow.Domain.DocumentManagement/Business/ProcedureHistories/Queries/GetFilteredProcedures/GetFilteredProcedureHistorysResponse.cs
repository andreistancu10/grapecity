using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetFilteredProcedures
{
    internal class GetFilteredProcedureHistoriesResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<ProcedureHistoryViewModel> Items { get; set; }
    }
}
