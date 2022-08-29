using DigitNow.Domain.DocumentManagement.Public.Forms.Models;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.DynamicForms.Commands
{
    public class SaveDynamicFormDataCommand : ICommand<ResultObject>
    {
        public long FormId { get; set; }
        public List<BasicRequestModel> Values { get; set; }
    }
}