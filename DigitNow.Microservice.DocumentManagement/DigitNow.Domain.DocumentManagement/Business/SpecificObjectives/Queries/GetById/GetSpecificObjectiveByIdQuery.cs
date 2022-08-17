using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.GetById
{
    public class GetSpecificObjectiveByIdQuery : IQuery<GetSpecificObjectiveByIdResponse>
    {
        public long ObjectiveId { get; set; }
        public GetSpecificObjectiveByIdQuery(long objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}
