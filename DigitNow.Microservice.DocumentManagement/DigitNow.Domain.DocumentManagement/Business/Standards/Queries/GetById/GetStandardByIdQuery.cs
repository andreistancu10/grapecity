using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetById
{
    public class GetStandardByIdQuery: IQuery<GetStandardByIdResponse>
    {
        public long Id { get; set; }
        public GetStandardByIdQuery(long id)
        {
            Id = id;
        }
    }
}
