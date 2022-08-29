using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdQuery :  IQuery<GetDynamicFormByIdResponse>
    {
        public long Id { get; set; }
    }
}