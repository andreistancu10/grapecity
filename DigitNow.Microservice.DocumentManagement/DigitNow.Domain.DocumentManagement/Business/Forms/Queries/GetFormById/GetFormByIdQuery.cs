using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Forms.Queries.GetFormById
{
    public class GetFormByIdQuery :  IQuery<GetFormByIdResponse>
    {
        public long Id { get; set; }
    }
}