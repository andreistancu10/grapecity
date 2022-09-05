using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Actions.Queries.GetById
{
    public class GetActionByIdQuery: IQuery<GetActionByIdResponse>
    {
        public long Id { get; set; }
        public GetActionByIdQuery(long id)
        {
            Id = id;
        }
    }
}
