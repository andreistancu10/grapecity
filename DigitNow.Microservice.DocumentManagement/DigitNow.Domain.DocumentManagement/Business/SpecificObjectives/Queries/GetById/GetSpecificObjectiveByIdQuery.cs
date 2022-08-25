using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById
{
    public class GetSpecificObjectiveByIdQuery : IQuery<SpecificObjectiveViewModel>
    {
        public long ObjectiveId { get; set; }
        public GetSpecificObjectiveByIdQuery(long objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}
