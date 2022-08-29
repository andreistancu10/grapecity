using DigitNow.Domain.DocumentManagement.Public.Common.Models;
using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands
{
    public class SaveDynamicFormDataCommand : ICommand<ResultObject>
    {
        public long DynamicFormId { get; set; }
        public List<KeyValueRequestModel> DynamicFormFillingValues { get; set; }
    }
}