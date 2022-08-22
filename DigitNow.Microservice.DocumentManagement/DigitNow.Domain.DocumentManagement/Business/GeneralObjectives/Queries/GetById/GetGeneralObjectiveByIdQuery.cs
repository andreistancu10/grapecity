using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.GeneralObjectives.Queries.GetById
{
    public class GetGeneralObjectiveByIdQuery : IQuery<GetGeneralObjectiveByIdResponse>
    {
        public long ObjectiveId { get; set; }
        public GetGeneralObjectiveByIdQuery(long objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}
