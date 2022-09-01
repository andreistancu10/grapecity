using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.Get
{
    public class GetSpecificObjectiveResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<SpecificObjectiveViewModel> Items { get; set; }
    }
}
