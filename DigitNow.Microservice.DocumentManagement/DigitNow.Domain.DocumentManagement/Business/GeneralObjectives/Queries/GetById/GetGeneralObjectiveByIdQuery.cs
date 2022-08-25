using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdQuery : IQuery<GeneralObjectiveViewModel>
    {
        public long ObjectiveId { get; set; }
        public GetGeneralObjectiveByIdQuery(long objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}
