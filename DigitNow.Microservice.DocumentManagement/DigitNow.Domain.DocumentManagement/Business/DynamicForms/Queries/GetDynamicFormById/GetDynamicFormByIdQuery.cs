using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Queries.GetDynamicFormById
{
    public class GetDynamicFormByIdQuery :  IQuery<DynamicFormViewModel>
    {
        public long FormId { get; set; }
        public long? FormFillingId { get; set; }
    }
}