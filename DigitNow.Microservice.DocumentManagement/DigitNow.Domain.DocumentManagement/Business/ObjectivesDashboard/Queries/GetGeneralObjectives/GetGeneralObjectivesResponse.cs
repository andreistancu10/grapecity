using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;

namespace DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives
{
    public class GetGeneralObjectivesResponse
    {
        public long TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public IList<GeneralObjectiveViewModel> Items { get; set; }
    }
}
