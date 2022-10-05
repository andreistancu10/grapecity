using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetFilteredProcedures
{
    internal class GetFilteredProceduresResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<ProcedureViewModel> Items { get; set; }
    }
}
