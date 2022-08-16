using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetById
{
    public class GetFormByIdQuery :  IQuery<GetFormByIdResponse>
    {
        public long Id { get; set; }
    }
}